using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProspectRetailerList : ContentPage
    {
        string contact;
        string host;
        string database;
        string ipaddress;
        byte[] pingipaddress;

        public ProspectRetailerList(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            InitializeComponent();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            this.pingipaddress = pingipaddress;
            Init();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }

        void Init()
        {
            GetProspectRetailer();
            searchCategory.SelectedIndex = 0;
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#1abc9c");
        }

        public void GetProspectRetailer()
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Coordinator=? ORDER BY FileAs ASC LIMIT 100", contact);
            var resultCount = getProspect.Result.Count;

            if (resultCount > 0)
            {
                var result = getProspect.Result;
                lstProspect.ItemsSource = result;
            }
        }

        private async void lstProspect_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        ContactsTable item = (ContactsTable)e.Item;

                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProspectRetailerDetails(item))
                        {
                            BarBackgroundColor = Color.FromHex("#1abc9c")
                        });
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }

        private void lstProspect_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstProspect.SelectedItem = null;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' ORDER BY FileAs ASC LIMIT 100");
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
            }
        }

        private void sbName_Activated(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' ORDER BY FileAs ASC LIMIT 100");
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
            }
        }

        private void sbStreet_Activated(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' ORDER BY PresStreet ASC LIMIT 100");
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
            }
        }

        private void sbBarangay_Activated(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' ORDER BY PresBarangay ASC LIMIT 100");
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
            }
        }

        private void sbDistrict_Activated(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' ORDER BY PresDistrict ASC LIMIT 100");
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
            }
        }

        private void sbCity_Activated(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' ORDER BY PresTown ASC LIMIT 100");
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
            }
        }

        private void sbProvince_Activated(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' ORDER BY PresProvince ASC LIMIT 100");
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
            }
        }

        private async void addProspect_Activated(object sender, EventArgs e)
        {
            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if (string.IsNullOrEmpty(appdate))
            {
                Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new AddProspectRetailer(host, database, contact, ipaddress, pingipaddress))
                        {
                            BarBackgroundColor = Color.FromHex("#1abc9c")
                        });
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }

        private void lstProspect_Refreshing(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' ORDER BY FileAs ASC LIMIT 100");
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 100";
                    var getProspect = conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstProspect.ItemsSource = result;
                    }
                }
            }

            lstProspect.EndRefresh();
        }
    }
}