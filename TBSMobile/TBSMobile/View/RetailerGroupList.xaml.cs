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
    public partial class RetailerGroupList : ContentPage
    {
        string contactID;
        string coordinator;

        public RetailerGroupList(ContactsTable item)
        {
            InitializeComponent();
            this.contactID = item.ContactID;
            this.coordinator = item.Coordinator;
            GetRetailerGroup(item.ContactID);
            searchCategory.SelectedIndex = 0;
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

        public void GetRetailerGroup(string contact)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getRetailer = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=? ORDER BY RetailerCode ASC", contact);
            var resultCount = getRetailer.Result.Count;

            if (resultCount > 0)
            {
                var result = getRetailer.Result;
                lstRetailerGroup.ItemsSource = result;
            }
        }

        private async void lstRetailerGroup_ItemTapped(object sender, ItemTappedEventArgs e)
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

                        RetailerGroupTable item = (RetailerGroupTable)e.Item;

                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new RetailerGroupDetails(item))
                        {
                            BarBackgroundColor = Color.FromHex("#e67e22")
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

        private void lstRetailerGroup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstRetailerGroup.SelectedItem = null;
        }

        private void lstRetailerGroup_Refreshing(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=? ORDER BY RetailerCode ASC LIMIT 50", contactID);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstRetailerGroup.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Code")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND RetailerCode LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
            }

            lstRetailerGroup.EndRefresh();
        }

        private void sbRCode_Activated(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=? ORDER BY RetailerCode ASC LIMIT 50", contactID);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstRetailerGroup.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Code")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND RetailerCode LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
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
                var getProspect = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=? ORDER BY PresStreet ASC LIMIT 50", contactID);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstRetailerGroup.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Code")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND RetailerCode LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
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
                var getProspect = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=? ORDER BY PresBarangay ASC LIMIT 50", contactID);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstRetailerGroup.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Code")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND RetailerCode LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
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
                var getProspect = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=? ORDER BY PresDistrict ASC LIMIT 50", contactID);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstRetailerGroup.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Code")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND RetailerCode LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
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
                var getProspect = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=? ORDER BY PresTown ASC LIMIT 50", contactID);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstRetailerGroup.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Code")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND RetailerCode LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
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
                var getProspect = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=? ORDER BY PresProvince ASC LIMIT 50", contactID);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstRetailerGroup.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Code")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND RetailerCode LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            if (Search.Text == "" || Search.Text == null)
            {
                var getProspect = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=? ORDER BY RetailerCode ASC LIMIT 50", contactID);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstRetailerGroup.ItemsSource = result;
                }
            }
            else
            {
                if (searchCategory.SelectedItem.ToString() == "Retailer Code")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND RetailerCode LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Street")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Barangay")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "District")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "City")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
                else if (searchCategory.SelectedItem.ToString() == "Province")
                {
                    var sql = "SELECT * FROM tblRetailerGroup WHERE ContactID = '" + contactID + "' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY RetailerCode ASC LIMIT 50";
                    var getProspect = conn.QueryAsync<RetailerGroupTable>(sql);
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailerGroup.ItemsSource = result;
                    }
                }
            }
        }
    }
}