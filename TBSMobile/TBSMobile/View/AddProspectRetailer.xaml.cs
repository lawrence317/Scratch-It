using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddProspectRetailer : ContentPage
	{
        string contact;
        string host;
        string database;
        string ipaddress;
        byte[] pingipaddress;

        public AddProspectRetailer (string host, string database, string contact, string ipaddress, byte[] pingipaddress)
		{
			InitializeComponent ();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            this.pingipaddress = pingipaddress;
            SetTempId();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var appdate = Preferences.Get("appdatetime", String.Empty, "private_prefs");

            if(string.IsNullOrEmpty(appdate))
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

        private async void btnPage1Next_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entFirstName.Text) || string.IsNullOrEmpty(entMiddleName.Text) || string.IsNullOrEmpty(entLastName.Text) || string.IsNullOrEmpty(entLandmark.Text) ||
                string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) || string.IsNullOrEmpty(entTownCode.Text) || string.IsNullOrEmpty(entProvinceCode.Text) ||
                string.IsNullOrEmpty(entCountry.Text))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Got it");

                if (string.IsNullOrEmpty(entFirstName.Text))
                {
                    firstnamevalidator.IsVisible = true;
                    FirstNameFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    firstnamevalidator.IsVisible = false;
                    FirstNameFrame.BorderColor = Color.FromHex("#e8eaed");
                }

                if (string.IsNullOrEmpty(entMiddleName.Text))
                {
                    middlenamevalidator.IsVisible = true;
                    MiddleNameFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    middlenamevalidator.IsVisible = false;
                    MiddleNameFrame.BorderColor = Color.FromHex("#e8eaed");
                }

                if (string.IsNullOrEmpty(entLastName.Text))
                {
                    lastnamevalidator.IsVisible = true;
                    LastNameFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    lastnamevalidator.IsVisible = false;
                    LastNameFrame.BorderColor = Color.FromHex("#e8eaed");
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

                            acPage1.IsVisible = false;
                            acPage2.IsVisible = true;
                            page1nav.IsVisible = false;
                            page2nav.IsVisible = true;
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

        private async void btnPage2Back_Clicked(object sender, EventArgs e)
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

                        acPage1.IsVisible = true;
                        acPage2.IsVisible = false;

                        page1nav.IsVisible = true;
                        page2nav.IsVisible = false;
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

        public void SetTempId()
        {
            var numbers = "0123456789";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < 2; i++)
            {
                stringChars[i] = numbers[random.Next(numbers.Length)];
            }
            for (int i = 2; i < 5; i++)
            {
                stringChars[i] = numbers[random.Next(numbers.Length)];
            }

            var finalString = new String(stringChars);
            entTempID.Text = "PR-" + finalString;
        }

        private async void btnAddContacts_Clicked(object sender, EventArgs e)
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

                        if(string.IsNullOrEmpty(entPhoto1Url.Text) || string.IsNullOrEmpty(entPhoto2Url.Text) || string.IsNullOrEmpty(entPhoto3Url.Text))
                        {
                            photovideovalidator.IsVisible = true;
                        }
                        else
                        {
                            photovideovalidator.IsVisible = false;

                            var confirm = await DisplayAlert("Sending Confirmation", "Are you sure you want to send this form?", "Yes", "No");

                            if (confirm == true)
                            {
                                btnBackPage2.IsEnabled = false;
                                btnAddContacts.IsEnabled = false;
                                
                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    var ping = new Ping();
                                    var reply = ping.Send(new IPAddress(pingipaddress), 5000);
                                    if (reply.Status == IPStatus.Success)
                                    {
                                        var optimalSpeed = 300000;
                                        var connectionTypes = CrossConnectivity.Current.ConnectionTypes;

                                        if (connectionTypes.Any(speed => Convert.ToInt32(speed) < optimalSpeed))
                                        {
                                            var sendconfirm = await DisplayAlert("Slow Connection Connection Warning", "Slow connection detected. Do you want to send the data?", "Send to server", "Save offline");
                                            if (sendconfirm == true)
                                            {
                                                Send_online();
                                            }
                                            else
                                            {
                                                Send_offline();
                                            }
                                        }
                                        else
                                        {
                                            Send_online();
                                        }
                                    }
                                    else
                                    {
                                        Send_offline();
                                    }
                                }
                                else
                                {
                                    Send_offline();
                                }
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

        private void entFirstName_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entFirstName.Text))
            {
                firstnamevalidator.IsVisible = true;
                FirstNameFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                firstnamevalidator.IsVisible = false;
                FirstNameFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }

        private void entMiddleName_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entMiddleName.Text))
            {
                middlenamevalidator.IsVisible = true;
                MiddleNameFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                middlenamevalidator.IsVisible = false;
                MiddleNameFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }

        private void entLastName_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entLastName.Text))
            {
                lastnamevalidator.IsVisible = true;
                LastNameFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                lastnamevalidator.IsVisible = false;
                LastNameFrame.BorderColor = Color.FromHex("#e8eaed");
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

        private async void btnCamera1_Clicked(object sender, EventArgs e)
        {
            var prospectID = entTempID.Text;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Available", "Got it");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = false,
                        Name = prospectID + "_IMG_01.png",
                        CompressionQuality = 50,
                        PhotoSize = PhotoSize.Medium
                    }
                );

                entPhoto1Url.Text = file.Path;

                if (!string.IsNullOrEmpty(entPhoto1Url.Text))
                {
                    btnCamera1.IsEnabled = false;
                    btnCamera1.BackgroundColor = Color.FromHex("#219150");
                    photovideovalidator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void btnCamera2_Clicked(object sender, EventArgs e)
        {
            var prospectID = entTempID.Text;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Available", "Got it");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = false,
                        Name = prospectID + "_IMG_02.png",
                        CompressionQuality = 50,
                        PhotoSize = PhotoSize.Medium
                    }
                );

                entPhoto2Url.Text = file.Path;

                if (!string.IsNullOrEmpty(entPhoto2Url.Text))
                {
                    btnCamera2.IsEnabled = false;
                    btnCamera2.BackgroundColor = Color.FromHex("#219150");
                    photovideovalidator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void btnCamera3_Clicked(object sender, EventArgs e)
        {
            var prospectID = entTempID.Text;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Available", "Got it");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = false,
                        Name = prospectID + "_IMG_03.png",
                        CompressionQuality = 50,
                        PhotoSize = PhotoSize.Medium
                    }
                );

                entPhoto3Url.Text = file.Path;

                if (!string.IsNullOrEmpty(entPhoto3Url.Text))
                {
                    btnCamera3.IsEnabled = false;
                    btnCamera3.BackgroundColor = Color.FromHex("#219150");
                    photovideovalidator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void btnCamera4_Clicked(object sender, EventArgs e)
        {
            var prospectID = entTempID.Text;
            TimeSpan time = new TimeSpan(0, 0, 0, 20, 0);

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Available", "Got it");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakeVideoAsync(
                    new Plugin.Media.Abstractions.StoreVideoOptions
                    {
                        Name = prospectID + "_VID.mp4",
                        CompressionQuality = 5,
                        Quality = VideoQuality.Low,
                        DesiredLength = time
                    }
                );

                entVideoUrl.Text = file.Path;

                if (!string.IsNullOrEmpty(entVideoUrl.Text))
                {
                    btnCamera4.IsEnabled = false;
                    btnCamera4.BackgroundColor = Color.FromHex("#219150");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
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
                    townPicker.SelectedIndex = 0;
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

        public async void Send_online()
        {
            try
            {
                var id = entTempID.Text;
                var firstName = entFirstName.Text;
                var middleName = entMiddleName.Text;
                var lastName = entLastName.Text;
                var fileas = firstName + " " + middleName + " " + lastName;
                var contactType = "Prospect Retailer";
                var street = entStreet.Text;
                var barangay = entBarangay.Text;
                var town = entTownCode.Text;
                var province = entProvinceCode.Text;
                var district = entDistrict.Text;
                var country = entCountry.Text;
                var landmark = entLandmark.Text;
                var telephone1 = entTelephone1.Text;
                var telephone2 = entTelephone2.Text;
                var mobile = entMobile.Text;
                var email = entMobile.Text;
                var photo1url = entPhoto1Url.Text;
                var photo2url = entPhoto2Url.Text;
                var photo3url = entPhoto3Url.Text;
                var videourl = entVideoUrl.Text;
                int employee = 0;
                int customer = 1;
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string url = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Request=9Fcq8C";
                string contentType = "application/json";
                JObject json = new JObject
                {
                    { "ContactID", id },
                    { "FirstName", firstName },
                    { "MiddleName", middleName },
                    { "LastName", lastName },
                    { "FileAs", fileas },
                    { "ContactType", contactType },
                    { "Street", street },
                    { "Barangay", barangay },
                    { "Town", town },
                    { "Province", province },
                    { "District", district },
                    { "Country", country },
                    { "Landmark", landmark },
                    { "Telephone1", telephone1 },
                    { "Telephone2", telephone2 },
                    { "Mobile", mobile },
                    { "Email", email },
                    { "MobilePhoto1", photo1url },
                    { "MobilePhoto2", photo2url },
                    { "MobilePhoto3", photo3url },
                    { "MobileVideo", videourl },
                    { "Employee", employee },
                    { "Customer", customer },
                    { "Coordinator", contact },
                    { "LastSync", current_datetime },
                    { "LastUpdated", current_datetime }
                };

                HttpClient client = new HttpClient();
                var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                if (response.IsSuccessStatusCode)
                {
                    byte[] Photo1Data = File.ReadAllBytes(photo1url);

                    var ph1link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=tWyd43";
                    string ph1contentType = "application/json";
                    JObject ph1json = new JObject
                    {
                        { "ContactID", id },
                        { "Photo1", Photo1Data }
                    };

                    HttpClient ph1client = new HttpClient();
                    var ph1response = await ph1client.PostAsync(ph1link, new StringContent(ph1json.ToString(), Encoding.UTF8, ph1contentType));

                    if (ph1response.IsSuccessStatusCode)
                    {
                        byte[] Photo2Data = File.ReadAllBytes(photo2url);

                        var ph2link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=qAWS26";
                        string ph2contentType = "application/json";
                        JObject ph2json = new JObject
                        {
                            { "ContactID", id },
                            { "Photo2", Photo2Data }
                        };

                        HttpClient ph2client = new HttpClient();
                        var ph2response = await ph2client.PostAsync(ph2link, new StringContent(ph2json.ToString(), Encoding.UTF8, ph2contentType));

                        if (ph2response.IsSuccessStatusCode)
                        {
                            byte[] Photo3Data = File.ReadAllBytes(photo3url);

                            var ph3link = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=XuY4RN";
                            string ph3contentType = "application/json";
                            JObject ph3json = new JObject
                            {
                                { "ContactID", id },
                                { "Photo3", Photo3Data }
                            };

                            HttpClient ph3client = new HttpClient();
                            var ph3response = await ph3client.PostAsync(ph3link, new StringContent(ph3json.ToString(), Encoding.UTF8, ph3contentType));

                            if (ph3response.IsSuccessStatusCode)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(videourl))
                                    {
                                        byte[] VideoData;

                                        if (!string.IsNullOrEmpty(videourl))
                                        {
                                            VideoData = File.ReadAllBytes(videourl);
                                        }
                                        else
                                        {
                                            VideoData = null;
                                        }

                                        var vidlink = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Contact=" + contact + "&Request=PsxQ7v";
                                        string vidcontentType = "application/json";
                                        JObject vidjson = new JObject
                                        {
                                           { "ContactID", id },
                                           { "Video", VideoData }
                                        };

                                        HttpClient vidclient = new HttpClient();
                                        var vidresponse = await vidclient.PostAsync(vidlink, new StringContent(vidjson.ToString(), Encoding.UTF8, vidcontentType));

                                        if (vidresponse.IsSuccessStatusCode)
                                        {
                                            var db = DependencyService.Get<ISQLiteDB>();
                                            var conn = db.GetConnection();

                                            var retailer = new ContactsTable
                                            {
                                                ContactID = id,
                                                FileAs = firstName + " " + lastName + " " + middleName,
                                                FirstName = firstName,
                                                MiddleName = middleName,
                                                LastName = lastName,
                                                ContactType = contactType,
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
                                                Photo1 = photo1url,
                                                Photo2 = photo2url,
                                                Photo3 = photo3url,
                                                Video = videourl,
                                                MobilePhoto1 = photo1url,
                                                MobilePhoto2 = photo2url,
                                                MobilePhoto3 = photo3url,
                                                MobileVideo = videourl,
                                                Employee = employee,
                                                Customer = customer,
                                                Coordinator = contact,
                                                LastSync = DateTime.Parse(current_datetime),
                                                LastUpdated = DateTime.Parse(current_datetime)
                                            };

                                            await conn.InsertAsync(retailer);

                                            await DisplayAlert("Data Sent", "Prospect retailer has been sent to the server", "Got it");
                                            await Application.Current.MainPage.Navigation.PopModalAsync();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                    Send_offline();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void Send_offline()
        {
            try
            {
                var id = entTempID.Text;
                var firstName = entFirstName.Text;
                var middleName = entMiddleName.Text;
                var lastName = entLastName.Text;
                var fileas = firstName + " " + middleName + " " + lastName;
                var contactType = "Prospect Retailer";
                var street = entStreet.Text;
                var barangay = entBarangay.Text;
                var town = entTownCode.Text;
                var province = entProvinceCode.Text;
                var district = entDistrict.Text;
                var country = entCountry.Text;
                var landmark = entLandmark.Text;
                var telephone1 = entTelephone1.Text;
                var telephone2 = entTelephone2.Text;
                var mobile = entMobile.Text;
                var email = entMobile.Text;
                var photo1url = entPhoto1Url.Text;
                var photo2url = entPhoto2Url.Text;
                var photo3url = entPhoto3Url.Text;
                var videourl = entVideoUrl.Text;
                int employee = 0;
                int customer = 1;
                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var prospect_insert = new ContactsTable
                {
                    ContactID = id,
                    FileAs = fileas,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    ContactType = contactType,
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
                    Photo1 = photo1url,
                    Photo2 = photo2url,
                    Photo3 = photo3url,
                    Video = videourl,
                    MobilePhoto1 = photo1url,
                    MobilePhoto2 = photo2url,
                    MobilePhoto3 = photo3url,
                    MobileVideo = videourl,
                    Employee = employee,
                    Customer = customer,
                    Coordinator = contact,
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await conn.InsertAsync(prospect_insert);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            await DisplayAlert("Offline Save", "Prospect retailer has been saved offline. Connect to the server to send your activity", "Got it");
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}