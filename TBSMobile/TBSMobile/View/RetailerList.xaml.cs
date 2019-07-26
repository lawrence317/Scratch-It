using Microsoft.AppCenter.Crashes;
using System;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RetailerList : ContentPage
	{
        string contact = Preferences.Get("contactid", String.Empty, "private_prefs");

        public RetailerList ()
		{
			InitializeComponent ();
            
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
                }
            }
        }

        public async void GetRetailer()
        {
            try
            {
                var getRetailer = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Supervisor=? ORDER BY FileAs ASC LIMIT 20", contact);
                var resultCount = getRetailer.Result.Count;

                if (resultCount > 0)
                {
                    var result = getRetailer.Result;
                    lstRetailer.ItemsSource = result;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void sbName_Activated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY FileAs ASC LIMIT 20");
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
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new AddRetailerOutlet(selected))
                            {
                                BarBackgroundColor = Color.FromHex("#e67e22")
                            });
                        }
                        else
                        {
                           await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                            await Navigation.PopToRootAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void sbStreet_Activated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresStreet ASC LIMIT 20");
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
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresStreet ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void sbBarangay_Activated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresBarangay ASC LIMIT 20");
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
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresBarangay ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void sbDistrict_Activated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresDistrict ASC LIMIT 20");
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
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresDistrict ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void sbCity_Activated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresTown ASC LIMIT 20");
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
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresTown ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void sbProvince_Activated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY PresProvince ASC LIMIT 20");
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
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY PresProvince ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY FileAs ASC LIMIT 20");
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
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void lstRetailer_Refreshing(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Search.Text))
                {
                    var getProspect = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' ORDER BY FileAs ASC LIMIT 20");
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
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND FileAs LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Outlet Name")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND Landmark LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Street")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresStreet LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Barangay")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresBarangay LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "District")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresDistrict LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "City")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresTown LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
                        var resultCount = getProspect.Result.Count;

                        if (resultCount > 0)
                        {
                            var result = getProspect.Result;
                            lstRetailer.ItemsSource = result;
                        }
                    }
                    else if (searchCategory.SelectedItem.ToString() == "Province")
                    {
                        var sql = "SELECT * FROM tblContacts WHERE RetailerType != 'RT00004' AND PresProvince LIKE '%" + Search.Text + "%' ORDER BY FileAs ASC LIMIT 20";
                        var getProspect = Constants.conn.QueryAsync<ContactsTable>(sql);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new RetailerGroupList(item))
                            {
                                BarBackgroundColor = Color.FromHex("#e67e22")
                            });
                        }
                        else
                        {
                           await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                            await Navigation.PopToRootAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private void lstRetailer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstRetailer.SelectedItem = null;
        }
    }

}