using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using System;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddRetailerOutlet : ContentPage
	{
        string host = Preferences.Get("host", String.Empty, "private_prefs");
        string database = Preferences.Get("database", String.Empty, "private_prefs");
        string domain = Preferences.Get("domain", String.Empty, "private_prefs");
        string apifolder = Preferences.Get("apifolder", String.Empty, "private_prefs");
        string contact = Preferences.Get("contactid", String.Empty, "private_prefs");
        string selected;

        public AddRetailerOutlet (string selected)
		{
			InitializeComponent ();
            this.selected = selected;

            SetTempRetailerCode();
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

                        if (!string.IsNullOrEmpty(selected))
                        {
                            entContact.Text = selected;

                            string sql = "SELECT * FROM tblContacts WHERE ContactID = '" + selected + "' AND Deleted != '1'";
                            var getContact = Constants.conn.QueryAsync<ContactsTable>(sql);
                            var resultCount = getContact.Result.Count;

                            if (resultCount > 0)
                            {
                                var result = getContact.Result[0];
                                NameSearch.Text = result.FileAs;
                            }

                            lstName.IsVisible = false;
                        }
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

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {

                try
                {
                    var result = await this.DisplayAlert("Confirm", "Are you sure you want to discard this form?", "Yes", "No");

                    if (result == true)
                    {
                        await this.Navigation.PopModalAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            });

            return true;
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
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

                        var confirm = await DisplayAlert("Sending Confirmation", "Are you sure you want to send this form?", "Yes", "No");

                        if (confirm == true)
                        {
                            namevalidator.IsVisible = false;
                            codevalidator.IsVisible = false;
                            outletnamevalidator.IsVisible = false;
                            streetvalidator.IsVisible = false;
                            barangayvalidator.IsVisible = false;
                            cityvalidator.IsVisible = false;
                            provincevalidator.IsVisible = false;
                            districtvalidator.IsVisible = false;
                            countryvalidator.IsVisible = false;

                            var id = entContact.Text;
                            var retailerCode = entRetailerCode.Text;
                            var street = entStreet.Text;
                            var barangay = entBarangay.Text;
                            var town = entTownCode.Text;
                            var province = entProvinceCode.Text;
                            var district = entDistrict.Text;
                            var country = entCountry.Text;
                            var landmark = entLandmark.Text;
                            var mobile = entMobile.Text;
                            var telephone1 = entTelephone1.Text;
                            var telephone2 = entTelephone2.Text;
                            var email = entEmail.Text;
                            var location = entLocation.Text;
                            var deleted = "0";
                            var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            var getUsername = Constants.conn.QueryAsync<UserTable>("SELECT UserID FROM tblUser WHERE ContactID = ? AND Deleted != '1'", contact);
                            var crresult = getUsername.Result[0];
                            var username = crresult.UserID;
                            var recordlog = "AB :" + username + "->" + contact + " " + current_datetime;

                            fafPage3.IsVisible = false;
                            sendstatusform.IsVisible = true;

                            sendStatus.Text = "Checking internet connection";
                            
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                await App.TodoManager.SendRetailerOutletDirectly(host, database, domain, apifolder, contact, SyncStatus, id, retailerCode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                                await App.TodoManager.OnSendCompleteModal(host, database, domain, contact);
                            }
                            else
                            {
                                await App.TodoManager.SaveRetailerOutletToLocalDatabaseFailed(host, database, domain, apifolder, contact, SyncStatus, id, retailerCode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, deleted, location, recordlog);
                                await App.TodoManager.OnSendCompleteModal(host, database, domain, contact);
                            }
                        }
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

        public void SetTempRetailerCode()
        {
            try
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var numbers = "0123456789";
                var stringChars = new char[10];
                var random = new Random();

                for (int i = 0; i < 2; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                for (int i = 2; i < 6; i++)
                {
                    stringChars[i] = numbers[random.Next(numbers.Length)];
                }
                for (int i = 6; i < 10; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);
                entRetailerCode.Text = "TP-" + finalString;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void NameSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            search();
        }

        private void NameSearch_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entContact.Text))
            {
                namevalidator.IsVisible = true;
                RetailerNameFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                namevalidator.IsVisible = false;
                RetailerNameFrame.BorderColor = Color.FromHex("#e8eaed");
            }

            lstName.IsVisible = false;
        }

        private async void lstName_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ContactsTable item = (ContactsTable)e.Item;

                NameSearch.Text = item.FileAs;
                lstName.IsVisible = false;

                var getCode = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE FileAs=?", NameSearch.Text);
                var resultCount = getCode.Result.Count;

                if (resultCount > 0)
                {
                    for (int i = 0; i < resultCount; i++)
                    {
                        var result = getCode.Result[i];
                        entContact.Text = result.ContactID.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private void NameSearch_Focused(object sender, FocusEventArgs e)
        {
            search();
        }

        public async void search()
        {
            try
            {
                var keyword = NameSearch.Text;

                if (!string.IsNullOrEmpty(keyword))
                {
                    lstName.IsVisible = true;

                    string sql = "SELECT * FROM tblContacts WHERE FileAs LIKE '%" + keyword + "%' AND RetailerType != 'RT00004' AND Deleted != '1' AND Supervisor='" + contact + "' ORDER BY FileAs LIMIT 3";
                    var getUser = Constants.conn.QueryAsync<ContactsTable>(sql);
                    var resultCount = getUser.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getUser.Result;
                        lstName.HeightRequest = (resultCount * 45);
                        lstName.ItemsSource = result;
                    }
                    else
                    {
                        lstName.IsVisible = false;
                    }
                }
                else
                {
                    lstName.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private void entLandmark_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entLandmark.Text))
            {
                outletnamevalidator.IsVisible = true;
                OutletNameFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                outletnamevalidator.IsVisible = false;
                OutletNameFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }

        private void entStreet_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entStreet.Text))
            {
                streetvalidator.IsVisible = true;
                StreetFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                streetvalidator.IsVisible = false;
                StreetFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }

        private void entBarangay_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entBarangay.Text))
            {
                barangayvalidator.IsVisible = true;
                BarangayFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                barangayvalidator.IsVisible = false;
                BarangayFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }
        
        private void entCountry_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entCountry.Text))
            {
                countryvalidator.IsVisible = true;
                CountryFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                countryvalidator.IsVisible = false;
                CountryFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }

        private void entContact_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(entContact.Text))
            {
                namevalidator.IsVisible = true;
                RetailerNameFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                namevalidator.IsVisible = false;
                RetailerNameFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }

        private void townPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (townPicker.SelectedIndex > -1)
                {
                    var pickedTown = townPicker.Items[townPicker.SelectedIndex];
                    string[] picked = pickedTown.Split(new char[] { '/' });
                    string tid = picked[0];

                    entTownCode.Text = tid;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void townPicker_Unfocused(object sender, FocusEventArgs e)
        {
            if (townPicker.SelectedIndex < 0)
            {
                cityvalidator.IsVisible = true;
                TownFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                cityvalidator.IsVisible = false;
                TownFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }

        public async void province_searchAsync()
        {
            try
            {
                var keyword = provinceSearch.Text;

                if (!string.IsNullOrEmpty(keyword))
                {
                    lstProvince.IsVisible = true;

                    var sql = "SELECT * FROM tblProvince WHERE Province LIKE '%" + keyword + "%' AND Deleted != '1' ORDER BY Province LIMIT 3";
                    var getProvince = Constants.conn.QueryAsync<ProvinceTable>(sql);
                    var resultCount = getProvince.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getProvince.Result;
                        lstProvince.HeightRequest = (resultCount * 45);
                        lstProvince.ItemsSource = result;
                    }
                    else
                    {
                        lstProvince.IsVisible = false;
                    }
                }
                else
                {
                    lstProvince.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private void provinceSearch_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entProvinceCode.Text))
            {
                provincevalidator.IsVisible = true;
                ProvinceFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                provincevalidator.IsVisible = false;
                ProvinceFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }

        private void provinceSearch_Focused(object sender, FocusEventArgs e)
        {
            province_searchAsync();
        }

        private void provinceSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            province_searchAsync();
        }

        private async void lstProvince_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ProvinceFrame.BorderColor = Color.FromHex("#e8eaed");
                provincevalidator.IsVisible = false;

                ProvinceTable item = (ProvinceTable)e.Item;

                provinceSearch.Text = item.Province;
                entProvinceCode.Text = item.ProvinceID;
                lstProvince.IsVisible = false;

                var getTown = Constants.conn.QueryAsync<TownTable>("SELECT * FROM tblTown WHERE ProvinceID=? AND Deleted != '1'", item.ProvinceID).Result;

                if (getTown != null && getTown.Count > 0)
                {
                    townPicker.ItemsSource = getTown;
                    townPicker.IsEnabled = true;
                    townPicker.SelectedIndex = 0;
                }
                else
                {
                    townPicker.IsEnabled = false;
                    townPicker.SelectedIndex = -1;
                    entTownCode.Text = null;
                }

                provincevalidator.IsVisible = false;
                ProvinceFrame.BorderColor = Color.FromHex("#e8eaed");

                cityvalidator.IsVisible = false;
                TownFrame.BorderColor = Color.FromHex("#e8eaed");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void BtnGotoPage2_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entContact.Text) || string.IsNullOrEmpty(entRetailerCode.Text) || string.IsNullOrEmpty(entLandmark.Text))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Ok");

                if (string.IsNullOrEmpty(entContact.Text))
                {
                    namevalidator.IsVisible = true;
                    RetailerNameFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    namevalidator.IsVisible = false;
                    RetailerNameFrame.BorderColor = Color.FromHex("#e8eaed");
                }

                if (string.IsNullOrEmpty(entRetailerCode.Text))
                {
                    codevalidator.IsVisible = true;
                    RetailerCodeFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    codevalidator.IsVisible = false;
                    RetailerCodeFrame.BorderColor = Color.FromHex("#e8eaed");
                }

                if (string.IsNullOrEmpty(entLandmark.Text))
                {
                    outletnamevalidator.IsVisible = true;
                    OutletNameFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    outletnamevalidator.IsVisible = false;
                    OutletNameFrame.BorderColor = Color.FromHex("#e8eaed");
                }
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        fafPage1.IsVisible = false;
                        fafPage2.IsVisible = true;
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

        private async void BtnGoBackToPage1_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                {
                    Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                    fafPage2.IsVisible = false;
                    fafPage1.IsVisible = true;
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

        private async void BtnGoBackToPage2_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                {
                    Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                    fafPage3.IsVisible = false;
                    fafPage2.IsVisible = true;
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

        private async void BtnGotoPage3_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) || string.IsNullOrEmpty(entTownCode.Text) ||
               string.IsNullOrEmpty(entProvinceCode.Text) || string.IsNullOrEmpty(entCountry.Text))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Ok");

                if (string.IsNullOrEmpty(entStreet.Text))
                {
                    streetvalidator.IsVisible = true;
                    StreetFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    streetvalidator.IsVisible = false;
                    StreetFrame.BorderColor = Color.FromHex("#e8eaed");
                }

                if (string.IsNullOrEmpty(entBarangay.Text))
                {
                    barangayvalidator.IsVisible = true;
                    BarangayFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    barangayvalidator.IsVisible = false;
                    BarangayFrame.BorderColor = Color.FromHex("#e8eaed");
                }

                if (townPicker.SelectedIndex < 0)
                {
                    cityvalidator.IsVisible = true;
                    TownFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    cityvalidator.IsVisible = false;
                    TownFrame.BorderColor = Color.FromHex("#e8eaed");
                }

                if (string.IsNullOrEmpty(entProvinceCode.Text))
                {
                    provincevalidator.IsVisible = true;
                    ProvinceFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    provincevalidator.IsVisible = false;
                    ProvinceFrame.BorderColor = Color.FromHex("#e8eaed");
                }

                if (string.IsNullOrEmpty(entCountry.Text))
                {
                    countryvalidator.IsVisible = true;
                    CountryFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    countryvalidator.IsVisible = false;
                    CountryFrame.BorderColor = Color.FromHex("#e8eaed");
                }
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        fafPage2.IsVisible = false;
                        fafPage3.IsVisible = true;
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

        private void SyncStatus(string status)
        {
            Device.BeginInvokeOnMainThread(() => {
                try
                {
                    sendStatus.Text = status;
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            });
        }
    }
}