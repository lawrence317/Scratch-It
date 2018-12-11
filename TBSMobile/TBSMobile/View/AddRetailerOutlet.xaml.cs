using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddRetailerOutlet : ContentPage
	{
        string contact;
        string host;
        string database;
        string ipaddress;
        byte[] pingipaddress;
        string selected;

        public AddRetailerOutlet (string host, string database, string contact, string ipaddress, byte[] pingipaddress, string selected)
		{
			InitializeComponent ();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            this.pingipaddress = pingipaddress;
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

                            var db = DependencyService.Get<ISQLiteDB>();
                            var conn = db.GetConnection();

                            string sql = "SELECT * FROM tblContacts WHERE ContactID = '" + selected + "'";
                            var getContact = conn.QueryAsync<ContactsTable>(sql);
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
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Confirm", "Are you sure you want to discard this form?", "Yes", "No");

                if (result == true)
                {
                    await this.Navigation.PopModalAsync();
                }
            });

            return true;
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entContact.Text) || string.IsNullOrEmpty(entRetailerCode.Text) || string.IsNullOrEmpty(entLandmark.Text) ||
               string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) || string.IsNullOrEmpty(entTownCode.Text) || 
               string.IsNullOrEmpty(entProvinceCode.Text) || string.IsNullOrEmpty(entCountry.Text))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Got it");

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

                                btnAdd.IsEnabled = false;
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
                                var DateTime.Parse(current_datetime) = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    var ping = new Ping();
                                    var reply = ping.Send(new IPAddress(pingipaddress), 1500);
                                    if (reply.Status == IPStatus.Success)
                                    {
                                        try
                                        {
                                            string url = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Request=Pb3c6A";
                                            string contentType = "application/json";
                                            JObject json = new JObject
                                            {
                                                { "ContactID", id },
                                                { "RetailerCode", retailerCode },
                                                { "PresStreet", street },
                                                { "PresBarangay", barangay },
                                                { "PresDistrict", district },
                                                { "PresTown", town },
                                                { "PresProvince", province },
                                                { "PresCountry", country },
                                                { "Landmark", landmark },
                                                { "Telephone1", telephone1 },
                                                { "Telephone2", telephone2 },
                                                { "Mobile", mobile },
                                                { "Email", email },
                                                { "GPSCoordinates", location },
                                                { "Coordinator", contact },
                                                { "LastSync", DateTime.Parse(current_datetime) },
                                                { "LastUpdated", DateTime.Parse(current_datetime) }
                                            };

                                            await DisplayAlert("Your retailer outlet was sent!", "Retailer outlet has been sent to the server", "Got it");
                                            await Application.Current.MainPage.Navigation.PopModalAsync();

                                            var db = DependencyService.Get<ISQLiteDB>();
                                            var conn = db.GetConnection();

                                            var retailer_group_insert = new RetailerGroupTable
                                            {
                                                ContactID = id,
                                                RetailerCode = retailerCode,
                                                PresStreet = street,
                                                PresBarangay = barangay,
                                                PresDistrict = district,
                                                PresTown = town,
                                                PresProvince = province,
                                                PresCountry = country,
                                                Landmark = landmark,
                                                Telephone1 = telephone1,
                                                Telephone2 = telephone2,
                                                Mobile = mobile,
                                                Email = email,
                                                GPSCoordinates = location,
                                                Coordinator = contact,
                                                LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime)),
                                            };

                                            await conn.InsertAsync(retailer_group_insert);

                                            HttpClient client = new HttpClient();
                                            var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                                        }
                                        catch (Exception ex)
                                        {
                                            Crashes.TrackError(ex);
                                        }
                                    }
                                    else
                                    {
                                        var db = DependencyService.Get<ISQLiteDB>();
                                        var conn = db.GetConnection();

                                        var retailer_group_insert = new RetailerGroupTable
                                        {
                                            ContactID = id,
                                            RetailerCode = retailerCode,
                                            PresStreet = street,
                                            PresBarangay = barangay,
                                            PresDistrict = district,
                                            PresTown = town,
                                            PresProvince = province,
                                            PresCountry = country,
                                            Landmark = landmark,
                                            Telephone1 = telephone1,
                                            Telephone2 = telephone2,
                                            Mobile = mobile,
                                            Email = email,
                                            GPSCoordinates = location,
                                            Coordinator = contact,
                                            LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime)),
                                        };

                                        await conn.InsertOrReplaceAsync(retailer_group_insert);

                                        await DisplayAlert("Retailer outlet was saved offline", "Retailer outlet has been saved offline connect to the server to send your activity", "Got it");
                                        await Application.Current.MainPage.Navigation.PopModalAsync();
                                    }
                                }
                                else
                                {
                                    var db = DependencyService.Get<ISQLiteDB>();
                                    var conn = db.GetConnection();

                                    var retailer_group_insert = new RetailerGroupTable
                                    {
                                        ContactID = id,
                                        RetailerCode = retailerCode,
                                        PresStreet = street,
                                        PresBarangay = barangay,
                                        PresDistrict = district,
                                        PresTown = town,
                                        PresProvince = province,
                                        PresCountry = country,
                                        Landmark = landmark,
                                        Telephone1 = telephone1,
                                        Telephone2 = telephone2,
                                        Mobile = mobile,
                                        Email = email,
                                        GPSCoordinates = location,
                                        Coordinator = contact,
                                        LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime)),
                                    };

                                    await conn.InsertOrReplaceAsync(retailer_group_insert);

                                    await DisplayAlert("Retailer outlet was saved offline", "Retailer outlet has been saved offline connect to the server to send your activity", "Got it");
                                    await Application.Current.MainPage.Navigation.PopModalAsync();
                                }
                            }
                        }
                        else
                        {
                            await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                            await Navigation.PopToRootAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
                
            }
        }

        public void SetTempRetailerCode()
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

        private void lstName_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ContactsTable item = (ContactsTable)e.Item;

                NameSearch.Text = item.FileAs;
                lstName.IsVisible = false;

                outletnamevalidator.IsVisible = false;
                OutletNameFrame.BorderColor = Color.FromHex("#e8eaed");

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getCode = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE FileAs=?", NameSearch.Text);
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
            }
        }

        private void NameSearch_Focused(object sender, FocusEventArgs e)
        {
            search();
        }

        public void search()
        {
            try
            {
                var keyword = NameSearch.Text;

                if (!string.IsNullOrEmpty(keyword))
                {
                    lstName.IsVisible = true;

                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    string sql = "SELECT * FROM tblContacts WHERE FileAs LIKE '%" + keyword + "%' AND ContactType='Retailer' AND Coordinator='" + contact + "' ORDER BY FileAs LIMIT 3";
                    var getUser = conn.QueryAsync<ContactsTable>(sql);
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
            if (townPicker.SelectedIndex > -1)
            {
                var pickedTown = townPicker.Items[townPicker.SelectedIndex];
                string[] picked = pickedTown.Split(new char[] { '/' });
                string tid = picked[0];

                entTownCode.Text = tid;
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

        public void province_search()
        {
            try
            {
                var keyword = provinceSearch.Text;

                if (!string.IsNullOrEmpty(keyword))
                {
                    lstProvince.IsVisible = true;

                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    string sql = "SELECT * FROM tblProvince WHERE Province LIKE '%" + keyword + "%' ORDER BY Province LIMIT 3";
                    var getProvince = conn.QueryAsync<ProvinceTable>(sql);
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
            province_search();
        }

        private void provinceSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            province_search();
        }

        private void lstProvince_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ProvinceFrame.BorderColor = Color.FromHex("#e8eaed");
                provincevalidator.IsVisible = false;

                ProvinceTable item = (ProvinceTable)e.Item;

                provinceSearch.Text = item.Province;
                entProvinceCode.Text = item.ProvinceID;
                lstProvince.IsVisible = false;

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getTown = conn.QueryAsync<TownTable>("SELECT * FROM tblTown WHERE ProvinceID=?", item.ProvinceID).Result;

                if (getTown != null && getTown.Count > 0)
                {
                    townPicker.ItemsSource = getTown;
                    townPicker.IsEnabled = true;
                    townPicker.SelectedIndex = -1;
                    entTownCode.Text = null;
                }
                else
                {
                    townPicker.IsEnabled = false;
                    townPicker.SelectedIndex = -1;
                    entTownCode.Text = null;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}