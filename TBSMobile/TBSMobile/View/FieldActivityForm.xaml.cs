using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Messaging;
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
    public partial class FieldActivityForm : ContentPage
    {
        string contact;
        string host;
        string database;
        string ipaddress;
        byte[] pingipaddress;

        public FieldActivityForm(string host, string database, string contact, string ipaddress, byte[] pingipaddress)
        {
            InitializeComponent();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            this.pingipaddress = pingipaddress;
            Init();
        }

        void Init()
        {
            tpTime.Text = DateTime.Now.ToString("hh:mm:ss");
            SetCAFNo();
            getRecipients();
            GetGPS();
            entEmployeeNumber.Text = contact;
            dpDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#3498db");
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
                    Crashes.TrackError(ex);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Confirm", "Are you sure you want to discard this form?", "Yes", "No");

                if (result == true)
                {
                    await this.Navigation.PopAsync();
                }
            });

            return true;
        }

        public void getRecipients()
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getRecipient = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID=?", contact);
                var resultCount = getRecipient.Result.Count;

                if (resultCount > 0)
                {
                    for (int i = 0; i < resultCount; i++)
                    {
                        var result = getRecipient.Result[i];
                        entrecipient.Text = result.Email;
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void SetCAFNo()
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
            entCafNo.Text = finalString;
        }

        public async void GetGPS()
        {
            Position position = null;

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 15;

            if (!locator.IsGeolocationAvailable)
            {
                await DisplayAlert("GPS Error", "GPS location not available", "Ok");
            }
            else if (!locator.IsGeolocationEnabled)
            {
                await DisplayAlert("GPS Error", "GPS location was not enabled", "Ok");
            }
            else
            {
                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(15), null, true);
                entLocation.Text = position.Latitude + "," + position.Longitude;
            }
        }

        private void NameSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            name_search();
        }

        private void NameSearch_Focused(object sender, FocusEventArgs e)
        {
            name_search();
        }

        private void NameSearch_Unfocused(object sender, FocusEventArgs e)
        {
            lstName.IsVisible = false;
        }

        private void lstName_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ContactsTable item = (ContactsTable)e.Item;

                entRetailer.Text = item.ContactID;
                NameSearch.Text = item.FileAs;
                lstName.IsVisible = false;

                entRetailerCode.Text = null;
                entEmployeeNumber.Text = null;
                entStreet.Text = null;
                entBarangay.Text = null;
                provinceSearch.Text = null;
                entProvinceCode.Text = null;
                entTownCode.Text = null;
                townPicker.SelectedIndex = -1;
                entDistrict.Text = null;
                entCountry.Text = null;
                entTelephone1.Text = null;
                entTelephone2.Text = null;
                entMobile.Text = null;
                entEmail.Text = null;
                entLandmark.Text = null;

                codePicker.IsEnabled = false;
                entStreet.IsEnabled = false;
                entBarangay.IsEnabled = false;
                provinceSearch.IsEnabled = false;
                townPicker.IsEnabled = false;
                entDistrict.IsEnabled = false;
                entCountry.IsEnabled = false;
                entTelephone1.IsEnabled = false;
                entTelephone2.IsEnabled = false;
                entMobile.IsEnabled = false;
                entEmail.IsEnabled = false;
                entLandmark.IsEnabled = false;
                btnGotoPage2.IsEnabled = false;

                outletvalidator.IsVisible = false;
                outletnamevalidator.IsVisible = false;
                streetvalidator.IsVisible = false;
                barangayvalidator.IsVisible = false;
                cityvalidator.IsVisible = false;
                provincevalidator.IsVisible = false;
                countryvalidator.IsVisible = false;

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getCode = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=?", item.ContactID);
                var resultCount = getCode.Result.Count;
                if (resultCount > 0)
                {
                    var result = getCode.Result;
                    codePicker.ItemsSource = result;
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

        public void name_search()
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
                        codePicker.IsEnabled = false;
                    }
                }
                else
                {
                    lstName.IsVisible = false;
                    codePicker.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void codePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (codePicker.SelectedIndex > -1)
                {
                    var pickedRetailerCode = codePicker.Items[codePicker.SelectedIndex];
                    string[] picked = pickedRetailerCode.Split(new char[] { '-' });
                    string code;

                    if (picked[0] == "TP")
                    {
                        code = picked[0] + "-" + picked[1];
                    }
                    else
                    {
                        code = picked[0];
                    }

                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var getCode = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode=?", code);
                    var resultCount = getCode.Result.Count;

                    entStreet.IsEnabled = true;
                    entBarangay.IsEnabled = true;
                    provinceSearch.IsEnabled = true;
                    townPicker.IsEnabled = true;
                    entDistrict.IsEnabled = true;
                    entCountry.IsEnabled = true;
                    entLandmark.IsEnabled = true;
                    entTelephone1.IsEnabled = true;
                    entTelephone2.IsEnabled = true;
                    entMobile.IsEnabled = true;
                    entEmail.IsEnabled = true;
                    btnGotoPage2.IsEnabled = true;

                    outletvalidator.IsVisible = false;
                    outletnamevalidator.IsVisible = false;
                    streetvalidator.IsVisible = false;
                    barangayvalidator.IsVisible = false;
                    cityvalidator.IsVisible = false;
                    provincevalidator.IsVisible = false;
                    countryvalidator.IsVisible = false;

                    if (resultCount > 0)
                    {
                        var result = getCode.Result.FirstOrDefault();
                        entRetailerCode.Text = result.RetailerCode;
                        entEmployeeNumber.Text = result.Coordinator;
                        entStreet.Text = result.PresStreet;
                        entBarangay.Text = result.PresBarangay;
                        entProvinceCode.Text = result.PresProvince;

                        var getProvincesql = "SELECT * FROM tblProvince WHERE ProvinceID = '" + result.PresProvince + "'";
                        var getProvince = conn.QueryAsync<ProvinceTable>(getProvincesql);
                        var provinceresultCount = getProvince.Result.Count;

                        if (provinceresultCount > 0)
                        {
                            var prvresult = getProvince.Result[0];
                            provinceSearch.Text = prvresult.Province;
                            lstProvince.IsVisible = false;
                        }

                        //var getProvince = conn.QueryAsync<ProvinceTable>("SELECT * FROM tblProvince").Result;

                        //if (getProvince != null && getProvince.Count > 0)
                        //{
                        //    provinceSearch.ItemsSource = getProvince;
                        //    var provincedata = (provinceSearch.ItemsSource as List<ProvinceTable>).FindIndex(p => p.ProvinceID == result.PresProvince);
                        //    provinceSearch.SelectedIndex = provincedata;

                        //    entProvinceCode.Text = result.PresProvince;

                        //    if (!string.IsNullOrEmpty(result.PresProvince))
                        //    {
                        //        townPicker.IsEnabled = true;
                        //    }
                        //    else
                        //    {
                        //        townPicker.IsEnabled = false;
                        //        entTownCode.Text = null;
                        //    } 
                        //}

                        if (!string.IsNullOrEmpty(result.PresProvince))
                        {
                            var getTown = conn.QueryAsync<TownTable>("SELECT * FROM tblTown WHERE ProvinceID=?", result.PresProvince).Result;

                            if (getTown != null && getTown.Count > 0)
                            {
                                townPicker.ItemsSource = getTown;

                                var towndata = (townPicker.ItemsSource as List<TownTable>).FindIndex(t => t.TownID == result.PresTown);
                                townPicker.SelectedIndex = towndata;

                                entTownCode.Text = result.PresTown;
                            }
                        }

                        entDistrict.Text = result.PresDistrict;

                        if (!string.IsNullOrEmpty(result.GPSCoordinates))
                        {
                            if (result.GPSCoordinates == "0.000,0.000")
                            {
                                GetGPS();
                            }
                            else
                            {
                                entLocation.Text = result.GPSCoordinates;
                            }
                        }
                        else
                        {
                            GetGPS();
                        }

                        if (string.IsNullOrEmpty(result.PresCountry))
                        {
                            entCountry.Text = "Philippines";
                        }
                        else
                        {
                            entCountry.Text = result.PresCountry;
                        }

                        entLandmark.Text = result.Landmark;
                        entTelephone1.Text = result.Telephone1;
                        entTelephone2.Text = result.Telephone2;
                        entMobile.Text = result.Mobile;
                        entEmail.Text = result.Email;
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            
        }

        private void swOthers_Toggled(object sender, ToggledEventArgs e)
        {
            if (swOthers.IsToggled == true)
            {
                OthersFrame.IsVisible = true;
                activityvalidator.IsVisible = false;
                entOthers.Focus();
            }
            else
            {
                OthersFrame.IsVisible = false;
                othersvalidator.IsVisible = false;

                if (swRekorida.IsToggled == false && swTradeCheck.IsToggled == false && swMerchandizing.IsToggled == false)
                {
                    activityvalidator.IsVisible = true;
                }
                else
                {
                    activityvalidator.IsVisible = false;
                }
            }
        }

        private async void btnCamera1_Clicked(object sender, EventArgs e)
        {
            var cafNo = entCafNo.Text;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Available", "Ok");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = false,
                        Name = cafNo + "_IMG_01.png",
                        CompressionQuality = 80,
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
            var cafNo = entCafNo.Text;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Available", "Ok");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = false,
                        Name = cafNo + "_IMG_02.png",
                        CompressionQuality = 80,
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
            var cafNo = entCafNo.Text;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Available", "Ok");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = false,
                        Name = cafNo + "_IMG_03.png",
                        CompressionQuality = 80,
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
            var cafNo = entCafNo.Text;
            TimeSpan time = new TimeSpan(0, 0, 0, 30, 0);

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await DisplayAlert("No Camera", "No Camera Available", "Ok");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakeVideoAsync(
                    new Plugin.Media.Abstractions.StoreVideoOptions
                    {
                        Name = cafNo + "_VID.mp4",
                        CompressionQuality = 0,
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

        private async void btnGotoPage2_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entRetailerCode.Text) || string.IsNullOrEmpty(entLandmark.Text) || string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) ||
                 string.IsNullOrEmpty(entCountry.Text) || string.IsNullOrEmpty(entTownCode.Text) || string.IsNullOrEmpty(entProvinceCode.Text))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Got it");

                if (codePicker.SelectedIndex < 0)
                {
                    outletvalidator.IsVisible = true;
                    OutletFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    outletvalidator.IsVisible = false;
                    OutletFrame.BorderColor = Color.FromHex("#e8eaed");
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

                            fafPage1.IsVisible = false;
                            fafPage2.IsVisible = true;
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

        private async void btnGoBacktoPage1_Clicked(object sender, EventArgs e)
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

                        fafPage2.IsVisible = false;
                        fafPage1.IsVisible = true;
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

        private async void btnGotoPage3_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entLocation.Text) || (swRekorida.IsToggled == false && swMerchandizing.IsToggled == false && swTradeCheck.IsToggled == false && swOthers.IsToggled == false) || (swOthers.IsToggled == true && string.IsNullOrEmpty(entOthers.Text)) || string.IsNullOrEmpty(entPhoto1Url.Text) || string.IsNullOrEmpty(entPhoto2Url.Text) || string.IsNullOrEmpty(entPhoto3Url.Text))
            {
                if (string.IsNullOrEmpty(entLocation.Text))
                {
                    await DisplayAlert("Form Required", "Please wait for the device to capture your location", "Got it");
                }

                if (swRekorida.IsToggled == false && swMerchandizing.IsToggled == false && swTradeCheck.IsToggled == false && swOthers.IsToggled == false)
                {
                    activityvalidator.IsVisible = true;
                }
                else
                {
                    activityvalidator.IsVisible = false;
                }

                if (swOthers.IsToggled == true && string.IsNullOrEmpty(entOthers.Text))
                {
                    othersvalidator.IsVisible = true;
                    OthersFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    othersvalidator.IsVisible = false;
                    OthersFrame.BorderColor = Color.FromHex("#e8eaed");
                }

                if (string.IsNullOrEmpty(entPhoto1Url.Text))
                {
                    photovideovalidator.IsVisible = true;
                }
                else
                {
                    photovideovalidator.IsVisible = false;
                }

                if (string.IsNullOrEmpty(entPhoto2Url.Text))
                {
                    photovideovalidator.IsVisible = true;
                }
                else
                {
                    photovideovalidator.IsVisible = false;
                }

                if (string.IsNullOrEmpty(entPhoto3Url.Text))
                {
                    photovideovalidator.IsVisible = true;
                }
                else
                {
                    photovideovalidator.IsVisible = false;
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

                            fafPage2.IsVisible = false;
                            fafPage3.IsVisible = true;
                            page2nav.IsVisible = false;
                            page3nav.IsVisible = true;
                            activityvalidator.IsVisible = false;
                            othersvalidator.IsVisible = false;
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

        private async void btnGoBacktoPage2_Clicked(object sender, EventArgs e)
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

                        fafPage3.IsVisible = false;
                        fafPage2.IsVisible = true;
                        page2nav.IsVisible = true;
                        page3nav.IsVisible = false;
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

        private async void btnSend_Clicked(object sender, EventArgs e)
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

                        try
                        {
                            if (confirm == true)
                            {
                                btnGoBacktoPage3.IsEnabled = false;
                                btnSend.IsEnabled = false;

                                var caf = entCafNo.Text;
                                var retailerCode = entRetailerCode.Text;
                                var employeeNumber = entEmployeeNumber.Text;
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
                                var email = entEmail.Text;
                                var location = entLocation.Text;
                                var date = dpDate.Text;
                                var startTime = tpTime.Text;
                                var endTime = DateTime.Now.ToString("hh:mm:ss");
                                var photo1url = entPhoto1Url.Text;
                                var photo2url = entPhoto2Url.Text;
                                var photo3url = entPhoto3Url.Text;
                                var videourl = entVideoUrl.Text;
                                var otherconcern = entOthers.Text;
                                var remarks = entRemarks.Text;
                                int rekorida;
                                int merchandizing;
                                int tradecheck;
                                int others;
                                var DateTime.Parse(current_datetime) = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                                if (swRekorida.IsToggled == true)
                                {
                                    rekorida = 1;
                                }
                                else
                                {
                                    rekorida = 0;
                                }

                                if (swMerchandizing.IsToggled == true)
                                {
                                    merchandizing = 1;
                                }
                                else
                                {
                                    merchandizing = 0;
                                }

                                if (swTradeCheck.IsToggled == true)
                                {
                                    tradecheck = 1;
                                }
                                else
                                {
                                    tradecheck = 0;
                                }

                                if (swOthers.IsToggled == true)
                                {
                                    others = 1;
                                }
                                else
                                {
                                    others = 0;
                                }

                                byte[] Photo1Data = File.ReadAllBytes(photo1url);
                                string photo1 = Convert.ToBase64String(Photo1Data);

                                byte[] Photo2Data = File.ReadAllBytes(photo2url);
                                string photo2 = Convert.ToBase64String(Photo2Data);

                                byte[] Photo3Data = File.ReadAllBytes(photo3url);
                                string photo3 = Convert.ToBase64String(Photo3Data);

                                string video;

                                if (!string.IsNullOrEmpty(videourl))
                                {
                                    byte[] VideoData = File.ReadAllBytes(videourl);
                                    video = Convert.ToBase64String(VideoData);
                                }
                                else
                                {
                                    video = "";
                                }

                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    var ping = new Ping();
                                    var reply = ping.Send(new IPAddress(pingipaddress), 1500);
                                    if (reply.Status == IPStatus.Success)
                                    {
                                        try
                                        {
                                            string url = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Request=Fsq6Tr";
                                            string contentType = "application/json";
                                            JObject json = new JObject
                                            {
                                                { "CAF", caf },
                                                { "CustomerID", retailerCode },
                                                { "EmployeeNumber", employeeNumber },
                                                { "Street", street },
                                                { "Barangay", barangay },
                                                { "Town", town },
                                                { "District", district },
                                                { "Province", province },
                                                { "Country", country },
                                                { "Landmark", landmark },
                                                { "Telephone1", telephone1 },
                                                { "Telephone2", telephone2 },
                                                { "Mobile", mobile },
                                                { "Email", email },
                                                { "Location", location },
                                                { "Date", DateTime.Parse(date) },
                                                { "StartTime", DateTime.Parse(startTime) },
                                                { "EndTime", DateTime.Parse(endTime) },
                                                { "Photo1", photo1 },
                                                { "Photo2", photo2 },
                                                { "Photo3", photo3 },
                                                { "Video", video },
                                                { "MobilePhoto1", photo1url },
                                                { "MobilePhoto2", photo2url },
                                                { "MobilePhoto3", photo3url },
                                                { "MobileVideo", videourl },
                                                { "Rekorida", rekorida },
                                                { "Merchandizing", merchandizing },
                                                { "TradeCheck", tradecheck },
                                                { "Others", others },
                                                { "OtherConcern", otherconcern },
                                                { "Remarks", remarks },
                                                { "LastUpdated", DateTime.Parse(DateTime.Parse(current_datetime)) }
                                            };

                                            HttpClient client = new HttpClient();
                                            client.Timeout = TimeSpan.FromMinutes(20);
                                            var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                                            var db = DependencyService.Get<ISQLiteDB>();
                                            var conn = db.GetConnection();

                                            string retailer_group_sql = "UPDATE tblRetailerGroup SET PresStreet = '" + street + "', PresBarangay = '" + barangay + "', PresTown = '" + town + "', PresProvince = '" + province + "', PresCountry = '" + country + "', PresDistrict= '" + district + "', Landmark = '" + landmark + "', Telephone1 = '" + telephone1 + "', Telephone2 = '" + telephone2 + "', Mobile = '" + mobile + "', Email = '" + email + "', GPSCoordinates = '" + location + "', LastSync = '" + DateTime.Parse(DateTime.Parse(current_datetime)) + "', LastUpdated = '" + DateTime.Parse(DateTime.Parse(current_datetime)) + "' WHERE RetailerCode = '" + retailerCode + "'";
                                            await conn.ExecuteAsync(retailer_group_sql);

                                            var caf_insert = new CAFTable
                                            {
                                                CAFNo = caf,
                                                EmployeeID = employeeNumber,
                                                CAFDate = DateTime.Parse(date),
                                                CustomerID = retailerCode,
                                                StartTime = DateTime.Parse(startTime),
                                                EndTime = DateTime.Parse(endTime),
                                                Photo1 = photo1url,
                                                Photo2 = photo2url,
                                                Photo3 = photo3url,
                                                Video = videourl,
                                                MobilePhoto1 = photo1url,
                                                MobilePhoto2 = photo2url,
                                                MobilePhoto3 = photo3url,
                                                MobileVideo = videourl,
                                                Remarks = remarks,
                                                OtherConcern = otherconcern,
                                                LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                            };

                                            await conn.InsertAsync(caf_insert);

                                            var rekorida_insert = new ActivityTable
                                            {
                                                CAFNo = caf,
                                                ContactID = employeeNumber,
                                                Activity = "Rekorida",
                                                ActivitySwitch = rekorida,
                                                LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                            };

                                            await conn.InsertAsync(rekorida_insert);

                                            var merchandizing_insert = new ActivityTable
                                            {
                                                CAFNo = caf,
                                                ContactID = employeeNumber,
                                                Activity = "Merchandizing",
                                                ActivitySwitch = merchandizing,
                                                LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                            };

                                            await conn.InsertAsync(merchandizing_insert);

                                            var trade_check_insert = new ActivityTable
                                            {
                                                CAFNo = caf,
                                                ContactID = employeeNumber,
                                                Activity = "Trade Check",
                                                ActivitySwitch = tradecheck,
                                                LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                            };

                                            await conn.InsertAsync(trade_check_insert);

                                            var others_insert = new ActivityTable
                                            {
                                                CAFNo = caf,
                                                ContactID = employeeNumber,
                                                Activity = "Others",
                                                ActivitySwitch = others,
                                                LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                            };

                                            await conn.InsertAsync(others_insert);

                                            await DisplayAlert("Your activity was sent!", "Your activity has been sent to the server", "Got it");
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

                                        var current = DateTime.Parse(DateTime.Parse(current_datetime));

                                        string retailer_group_sql = "UPDATE tblRetailerGroup SET PresStreet = '" + street + "', PresBarangay = '" + barangay + "', PresTown = '" + town + "', PresProvince = '" + province + "', PresCountry = '" + country + "', PresDistrict= '" + district + "', Landmark = '" + landmark + "', Telephone1 = '" + telephone1 + "', Telephone2 = '" + telephone2 + "', Mobile = '" + mobile + "', Email = '" + email + "', GPSCoordinates = '" + location + "',  LastUpdated = '" + current + "' WHERE RetailerCode = '" + retailerCode + "'";
                                        await conn.ExecuteAsync(retailer_group_sql);

                                        var caf_insert = new CAFTable
                                        {
                                            CAFNo = caf,
                                            EmployeeID = employeeNumber,
                                            CAFDate = DateTime.Parse(date),
                                            CustomerID = retailerCode,
                                            StartTime = DateTime.Parse(startTime),
                                            EndTime = DateTime.Parse(endTime),
                                            Photo1 = photo1url,
                                            Photo2 = photo2url,
                                            Photo3 = photo3url,
                                            Video = videourl,
                                            MobilePhoto1 = photo1url,
                                            MobilePhoto2 = photo2url,
                                            MobilePhoto3 = photo3url,
                                            MobileVideo = videourl,
                                            Remarks = remarks,
                                            OtherConcern = otherconcern,
                                            LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                        };

                                        await conn.InsertAsync(caf_insert);

                                        var rekorida_insert = new ActivityTable
                                        {
                                            CAFNo = caf,
                                            ContactID = employeeNumber,
                                            Activity = "Rekorida",
                                            ActivitySwitch = rekorida,
                                            LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                        };

                                        await conn.InsertAsync(rekorida_insert);

                                        var merchandizing_insert = new ActivityTable
                                        {
                                            CAFNo = caf,
                                            ContactID = employeeNumber,
                                            Activity = "Merchandizing",
                                            ActivitySwitch = merchandizing,
                                            LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                        };

                                        await conn.InsertAsync(merchandizing_insert);

                                        var trade_check_insert = new ActivityTable
                                        {
                                            CAFNo = caf,
                                            ContactID = employeeNumber,
                                            Activity = "Trade Check",
                                            ActivitySwitch = tradecheck,
                                            LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                        };

                                        await conn.InsertAsync(trade_check_insert);

                                        var others_insert = new ActivityTable
                                        {
                                            CAFNo = caf,
                                            ContactID = employeeNumber,
                                            Activity = "Others",
                                            ActivitySwitch = others,
                                            LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                        };

                                        await conn.InsertAsync(others_insert);

                                        await DisplayAlert("Your activity was saved offline", "Your activity has been saved offline connect to the server to send your activity", "Got it");
                                    }
                                }
                                else
                                {
                                    var db = DependencyService.Get<ISQLiteDB>();
                                    var conn = db.GetConnection();

                                    var current = DateTime.Parse(DateTime.Parse(current_datetime));

                                    string retailer_group_sql = "UPDATE tblRetailerGroup SET PresStreet = '" + street + "', PresBarangay = '" + barangay + "', PresTown = '" + town + "', PresProvince = '" + province + "', PresCountry = '" + country + "', PresDistrict= '" + district + "', Landmark = '" + landmark + "', Telephone1 = '" + telephone1 + "', Telephone2 = '" + telephone2 + "', Mobile = '" + mobile + "', Email = '" + email + "', GPSCoordinates = '" + location + "', LastUpdated = '" + current + "' WHERE RetailerCode = '" + retailerCode + "'";
                                    await conn.ExecuteAsync(retailer_group_sql);

                                    var caf_insert = new CAFTable
                                    {
                                        CAFNo = caf,
                                        EmployeeID = employeeNumber,
                                        CAFDate = DateTime.Parse(date),
                                        CustomerID = retailerCode,
                                        StartTime = DateTime.Parse(startTime),
                                        EndTime = DateTime.Parse(endTime),
                                        Photo1 = photo1url,
                                        Photo2 = photo2url,
                                        Photo3 = photo3url,
                                        Video = videourl,
                                        MobilePhoto1 = photo1url,
                                        MobilePhoto2 = photo2url,
                                        MobilePhoto3 = photo3url,
                                        MobileVideo = videourl,
                                        Remarks = remarks,
                                        OtherConcern = otherconcern,
                                        LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                    };

                                    await conn.InsertAsync(caf_insert);

                                    var rekorida_insert = new ActivityTable
                                    {
                                        CAFNo = caf,
                                        ContactID = employeeNumber,
                                        Activity = "Rekorida",
                                        ActivitySwitch = rekorida,
                                        LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                    };

                                    await conn.InsertAsync(rekorida_insert);

                                    var merchandizing_insert = new ActivityTable
                                    {
                                        CAFNo = caf,
                                        ContactID = employeeNumber,
                                        Activity = "Merchandizing",
                                        ActivitySwitch = merchandizing,
                                        LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                    };

                                    await conn.InsertAsync(merchandizing_insert);

                                    var trade_check_insert = new ActivityTable
                                    {
                                        CAFNo = caf,
                                        ContactID = employeeNumber,
                                        Activity = "Trade Check",
                                        ActivitySwitch = tradecheck,
                                        LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                    };

                                    await conn.InsertAsync(trade_check_insert);

                                    var others_insert = new ActivityTable
                                    {
                                        CAFNo = caf,
                                        ContactID = employeeNumber,
                                        Activity = "Others",
                                        ActivitySwitch = others,
                                        LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                    };

                                    await conn.InsertAsync(others_insert);

                                    await DisplayAlert("Your activity was saved offline", "Your activity has been saved offline connect to the server to send your activity", "Got it");
                                }

                                var recipients = entrecipient.Text;

                                if (!string.IsNullOrEmpty(recipients))
                                {
                                    if (CrossConnectivity.Current.IsConnected)
                                    {
                                        try
                                        {
                                            string url2 = "http://" + ipaddress + Constants.requestUrl + "Host=" + host + "&Database=" + database + "&Request=kcZw9g";
                                            string contentType2 = "application/json";
                                            JObject json2 = new JObject
                                            {
                                                { "ContactID", contact },
                                                { "Email", recipients },
                                                { "LastSync", DateTime.Parse(DateTime.Parse(current_datetime)) },
                                                { "LastUpdated", DateTime.Parse(DateTime.Parse(current_datetime)) }
                                            };

                                            HttpClient client2 = new HttpClient();
                                            var response2 = await client2.PostAsync(url2, new StringContent(json2.ToString(), Encoding.UTF8, contentType2));

                                            var db = DependencyService.Get<ISQLiteDB>();
                                            var conn = db.GetConnection();

                                            var getCode = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID=?", contact);
                                            var resultCount = getCode.Result.Count;
                                            if (resultCount > 0)
                                            {
                                                var email_update = new UserEmailTable
                                                {
                                                    ContactID = employeeNumber,
                                                    Email = recipients,
                                                    LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                    LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                                };

                                                await conn.InsertOrReplaceAsync(email_update);
                                            }
                                            else
                                            {
                                                var insert_email = new UserEmailTable
                                                {
                                                    ContactID = employeeNumber,
                                                    Email = recipients,
                                                    LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                    LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                                };

                                                await conn.InsertOrReplaceAsync(insert_email);
                                            }
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

                                        var getCode = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail");
                                        var resultCount = getCode.Result.Count;
                                        if (resultCount > 0)
                                        {
                                            var email_update = new UserEmailTable
                                            {
                                                ContactID = employeeNumber,
                                                Email = recipients,
                                                LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                            };

                                            await conn.InsertOrReplaceAsync(email_update);
                                        }
                                        else
                                        {
                                            var insert_email = new UserEmailTable
                                            {
                                                ContactID = employeeNumber,
                                                Email = recipients,
                                                LastSync = DateTime.Parse(DateTime.Parse(current_datetime)),
                                                LastUpdated = DateTime.Parse(DateTime.Parse(current_datetime))
                                            };

                                            await conn.InsertOrReplaceAsync(insert_email);
                                        }
                                    }

                                    List<string> arrayfromEntry = new List<string>();
                                    if (recipients.Contains(" ") == true)
                                    {
                                        arrayfromEntry = recipients.Split(new char[] { ' ' }).ToList();
                                    }
                                    else
                                    {
                                        arrayfromEntry = recipients.Split(new char[] { ',' }).ToList();
                                    }

                                    for (int i = 0; i < arrayfromEntry.Count(); i++)
                                    {
                                        arrayfromEntry[i] = '"' + arrayfromEntry[i] + '"';
                                    }

                                    string f = (string.Join(", ", arrayfromEntry));
                                    f = f.Remove(f.Count() - 1, 1);
                                    f = f + '"';

                                    var subject = "Field Activity Form - " + caf + " as of " + date;
                                    var body = "Good Day! Field Activity Form details has been sent to you.<br/><br/>" +
                                        "<b>Retailer:</b> " + NameSearch.Text + "<br/>" +
                                        "<b>Retailer Code:</b> " + retailerCode + "<br/>" +
                                        "<b>Employee:</b> " + employeeNumber + "<br/>" +
                                        "<b>Street:</b> " + street + "<br/>" +
                                        "<b>Barangay:</b> " + barangay + "<br/>" +
                                        "<b>Town/City/Municipality:</b> " + town + "<br/>" +
                                        "<b>Province:</b> " + province + "<br/>" +
                                        "<b>District:</b> " + district + "<br/>" +
                                        "<b>Country:</b> " + country + "<br/>" +
                                        "<b>On-site Location:</b> " + location + "<br/>" +
                                        "<b>Activity Date:</b> " + date + "<br/>" +
                                        "<b>Activity Start Time:</b> " + startTime + "<br/>" +
                                        "<b>Activity End Time:</b> " + endTime + "<br/>" +
                                        "<b>Rekorida:</b> " + rekorida + "<br/>" +
                                        "<b>Merchandizing:</b> " + merchandizing + "<br/>" +
                                        "<b>Trade Check:</b> " + tradecheck + "<br/>" +
                                        "<b>Others:</b> " + others + "<br/>" +
                                        "<b>Other Concern:</b> " + otherconcern + "<br/>" +
                                        "<b>Remarks:</b> " + remarks;

                                    var emailMessenger = CrossMessaging.Current.EmailMessenger;
                                    if (emailMessenger.CanSendEmail)
                                    {
                                        if (string.IsNullOrEmpty(videourl))
                                        {
                                            var emailsend = new EmailMessageBuilder()
                                            .To(f)
                                            .Subject(subject)
                                            .BodyAsHtml(body)
                                            .WithAttachment(photo1url, "image/png")
                                            .WithAttachment(photo2url, "image/png")
                                            .WithAttachment(photo3url, "image/png")
                                            .WithAttachment(videourl, "video/mp4")
                                            .Build();

                                            emailMessenger.SendEmail(emailsend);
                                        }
                                        else
                                        {
                                            var emailsend = new EmailMessageBuilder()
                                            .To(f)
                                            .Subject(subject)
                                            .BodyAsHtml(body)
                                            .WithAttachment(photo1url, "image/png")
                                            .WithAttachment(photo2url, "image/png")
                                            .WithAttachment(photo3url, "image/png")
                                            .Build();

                                            emailMessenger.SendEmail(emailsend);
                                        }
                                    }

                                    if (swFillup.IsToggled == false)
                                    {
                                        await Application.Current.MainPage.Navigation.PopAsync();
                                    }
                                    else if (swFillup.IsToggled == true)
                                    {
                                        await Application.Current.MainPage.Navigation.PopAsync();
                                        await Navigation.PushAsync(new FieldActivityForm(host, database, contact, ipaddress, pingipaddress));
                                    }
                                }
                                else
                                {
                                    if (swFillup.IsToggled == false)
                                    {
                                        await Application.Current.MainPage.Navigation.PopAsync();
                                    }
                                    else if (swFillup.IsToggled == true)
                                    {
                                        await Application.Current.MainPage.Navigation.PopAsync();
                                        await Navigation.PushAsync(new FieldActivityForm(host, database, contact, ipaddress, pingipaddress));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
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

        private void codePicker_Unfocused(object sender, FocusEventArgs e)
        {
            if (codePicker.SelectedIndex < 0)
            {
                outletvalidator.IsVisible = true;
                OutletFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                outletvalidator.IsVisible = false;
                OutletFrame.BorderColor = Color.FromHex("#e8eaed");
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

        private void Activity_Toggled(object sender, ToggledEventArgs e)
        {
            if (swRekorida.IsToggled == true || swMerchandizing.IsToggled == true || swTradeCheck.IsToggled == true || swOthers.IsToggled == true)
            {
                activityvalidator.IsVisible = false;
            }
            else
            {
                activityvalidator.IsVisible = true;
            }
        }

        private void entOthers_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entOthers.Text))
            {
                othersvalidator.IsVisible = true;
                OthersFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                othersvalidator.IsVisible = false;
                OthersFrame.BorderColor = Color.FromHex("#e8eaed");
            }
        }

        private async void addRetailerOutlet_Activated(object sender, EventArgs e)
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

                        var selected = entRetailer.Text;

                        Analytics.TrackEvent("Opened Add Retailer Outlet");

                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new AddRetailerOutlet(host, database, contact, ipaddress, pingipaddress, selected))
                        {
                            BarBackgroundColor = Color.FromHex("#3498db")
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
                    Crashes.TrackError(ex);
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(entRetailer.Text))
                {
                    codePicker.IsEnabled = false;

                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var getCode = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=?", entRetailer.Text);
                    var resultCount = getCode.Result.Count;

                    if (resultCount > 0)
                    {
                        var result = getCode.Result;
                        codePicker.ItemsSource = result;
                    }
                    else
                    {
                        lstName.IsVisible = false;
                    }

                    codePicker.Focus();
                    codePicker.Unfocus();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
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