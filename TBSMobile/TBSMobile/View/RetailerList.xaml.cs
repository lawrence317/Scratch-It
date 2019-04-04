using Microsoft.AppCenter.Crashes;
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
	public partial class RetailerList : ContentPage
	{
        string contact;
        string host;
        string database;
        string ipaddress;

        public RetailerList (string host, string database, string contact, string ipaddress)
		{
			InitializeComponent ();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            
            GetRetailer();
            searchCategory.SelectedIndex = 0;
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#e67e22");
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
                    Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
                }
            }
        }

        public void GetRetailer()
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getRetailer = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Supervisor=? ORDER BY FileAs ASC LIMIT 50", contact);
                var resultCount = getRetailer.Result.Count;

                if (resultCount > 0)
                {
                    var result = getRetailer.Result;
                    lstRetailer.ItemsSource = result;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private void sbName_Activated(object sender, EventArgs e)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY FileAs ASC LIMIT 50");
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailer.ItemsSource = result;
                    }
                }
                else
                {
                    if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private async void addRetailerOutlet_Activated(object sender, EventArgs e)
        {
            try
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

                            string selected = "";

                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new AddRetailerOutlet(host, database, contact, ipaddress, selected))
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
                        Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private void sbStreet_Activated(object sender, EventArgs e)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresStreet ASC LIMIT 50");
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailer.ItemsSource = result;
                    }
                }
                else
                {
                    if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private void sbBarangay_Activated(object sender, EventArgs e)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresBarangay ASC LIMIT 50");
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailer.ItemsSource = result;
                    }
                }
                else
                {
                    if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private void sbDistrict_Activated(object sender, EventArgs e)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresDistrict ASC LIMIT 50");
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailer.ItemsSource = result;
                    }
                }
                else
                {
                    if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private void sbCity_Activated(object sender, EventArgs e)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresTown ASC LIMIT 50");
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailer.ItemsSource = result;
                    }
                }
                else
                {
                    if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private void sbProvince_Activated(object sender, EventArgs e)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresProvince ASC LIMIT 50");
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailer.ItemsSource = result;
                    }
                }
                else
                {
                    if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY FileAs ASC LIMIT 50");
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailer.ItemsSource = result;
                    }
                }
                else
                {
                    if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private void lstRetailer_Refreshing(object sender, EventArgs e)
        {
            try
            {

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY FileAs ASC LIMIT 50");
                    var resultCount = getProspect.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProspect.Result;
                        lstRetailer.ItemsSource = result;
                    }
                }
                else
                {
                    if (searchCategory.SelectedItem.ToString() == "Retailer Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 50";
                        var getProspect = conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                }

                lstRetailer.EndRefresh();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private async void lstRetailer_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
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

                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new RetailerGroupList(item, host, database, contact, ipaddress))
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
                        Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }

        private void lstRetailer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstRetailer.SelectedItem = null;
        }
    }

}