using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
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
using System.Net.Sockets;
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
        

        public FieldActivityForm(string host, string database, string contact, string ipaddress)
        {
            InitializeComponent();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            
            Init();
        }

        void Init()
        {
            tpTime.Text = DateTime.Now.ToString("HH:mm:ss");
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
                   await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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

        public async void getRecipients()
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
               await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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
                stringChars[i] = chars[random.Next(numbers.Length)];
            }
            for (int i = 2; i < 6; i++)
            {
                stringChars[i] = numbers[random.Next(numbers.Length)];
            }
            for (int i = 6; i < 10; i++)
            {
                stringChars[i] = chars[random.Next(numbers.Length)];
            }

            var finalString = new String(stringChars);
            entCafNo.Text = "AF" + finalString;
        }

        public async void GetGPS()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                if (!locator.IsGeolocationAvailable)
                {
                    await DisplayAlert("GPS Error", "GPS location not available", "Got it");
                }
                else if (!locator.IsGeolocationEnabled)
                {
                    await DisplayAlert("GPS Error", "GPS location is not enabled", "Got it");
                }
                else
                {
                    position = await locator.GetPositionAsync(TimeSpan.FromMinutes(10), null, true);
                    
                    string location = position.Latitude + "," + position.Longitude;
                    entOnsiteLocation.Text = location;
                    entLocation.Text = location;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("GPS Error", "Unable to get location " + ex, "Ok");
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

        private async void lstName_ItemTapped(object sender, ItemTappedEventArgs e)
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
               await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public async void name_search()
        {
            try
            {
                var keyword = NameSearch.Text;

                if (!string.IsNullOrEmpty(keyword))
                {
                    lstName.IsVisible = true;

                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    string sql = "SELECT * FROM tblContacts WHERE FileAs LIKE '%" + keyword + "%' AND RetailerType != 'RT00004' AND Deleted != '1' AND Supervisor='" + contact + "' ORDER BY FileAs LIMIT 3";
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
               await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private async void codePicker_SelectedIndexChanged(object sender, EventArgs e)
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

                    var getCode = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode=? AND Deleted != '1'", code);
                    var resultCount = getCode.Result.Count;

                    entStreet.IsEnabled = true;
                    entBarangay.IsEnabled = true;
                    provinceSearch.IsEnabled = true;
                    townPicker.IsEnabled = true;
                    entDistrict.IsEnabled = true;
                    entLandmark.IsEnabled = true;
                    btnGotoPage2.IsEnabled = true;
                    entCountry.IsEnabled = false;
                    entTelephone1.IsEnabled = true;
                    entTelephone2.IsEnabled = true;
                    entMobile.IsEnabled = true;
                    entEmail.IsEnabled = true;
                    entLandmark.IsEnabled = true;

                    outletvalidator.IsVisible = false;
                    outletnamevalidator.IsVisible = false;
                    streetvalidator.IsVisible = false;
                    barangayvalidator.IsVisible = false;
                    cityvalidator.IsVisible = false;
                    provincevalidator.IsVisible = false;
                    countryvalidator.IsVisible = false;

                    entLocation.Text = null;

                    if (resultCount > 0)
                    {
                        var result = getCode.Result.FirstOrDefault();
                        entRetailerCode.Text = result.RetailerCode;
                        entEmployeeNumber.Text = result.Supervisor;
                        entStreet.Text = result.PresStreet;
                        entBarangay.Text = result.PresBarangay;
                        entProvinceCode.Text = result.PresProvince;

                        var getProvincesql = "SELECT * FROM tblProvince WHERE ProvinceID = '" + result.PresProvince + "' AND Deleted != '1'";
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
                            entLocation.Text = result.GPSCoordinates;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(entOnsiteLocation.Text))
                            {
                                GetGPS();
                            }
                            else
                            {
                                entLocation.Text = entOnsiteLocation.Text;
                            }
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
               await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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
                await DisplayAlert("No Camera", "No Camera Available", "Got it");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = false,
                        Name = cafNo + "_IMG_01.png",
                        CompressionQuality = 50,
                        PhotoSize = PhotoSize.Medium
                    }
                );

                if (file == null)
                    return;

                entPhoto1Url.Text = file.Path;

                if (!string.IsNullOrEmpty(entPhoto1Url.Text))
                {
                    btnCamera1.IsVisible = false;
                    photo1thumb.IsVisible = true;
                    photo1thumb.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                    photovideovalidator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private async void btnCamera2_Clicked(object sender, EventArgs e)
        {
            var cafNo = entCafNo.Text;

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
                        Name = cafNo + "_IMG_02.png",
                        CompressionQuality = 50,
                        PhotoSize = PhotoSize.Medium
                    }
                );

                if (file == null)
                    return;

                entPhoto2Url.Text = file.Path;

                if (!string.IsNullOrEmpty(entPhoto2Url.Text))
                {
                    btnCamera2.IsVisible = false;
                    photo2thumb.IsVisible = true;
                    photo2thumb.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                    photovideovalidator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private async void btnCamera3_Clicked(object sender, EventArgs e)
        {
            var cafNo = entCafNo.Text;

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
                        Name = cafNo + "_IMG_03.png",
                        CompressionQuality = 50,
                        PhotoSize = PhotoSize.Medium
                    }
                );

                if (file == null)
                    return;

                entPhoto3Url.Text = file.Path;

                if (!string.IsNullOrEmpty(entPhoto3Url.Text))
                {
                    btnCamera3.IsVisible = false;
                    photo3thumb.IsVisible = true;
                    photo3thumb.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                    photovideovalidator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private async void btnCamera4_Clicked(object sender, EventArgs e)
        {
            var cafNo = entCafNo.Text;
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
                        Name = cafNo + "_VID.mp4",
                        CompressionQuality = 5,
                        Quality = VideoQuality.Low,
                        DesiredLength = time
                    }
                );

                if (file == null)
                    return;

                entVideoUrl.Text = file.Path;

                if (!string.IsNullOrEmpty(entVideoUrl.Text))
                {
                    btnCamera4.IsVisible = false;
                    vidthumb.IsVisible = true;
                    vidthumb.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        private async void BtnGoBacktoPage3_Clicked(object sender, EventArgs e)
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

                        fafPage4.IsVisible = false;
                        fafPage3.IsVisible = true;
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        private async void BtnGoBacktoPage4_Clicked(object sender, EventArgs e)
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

                        fafPage5.IsVisible = false;
                        fafPage4.IsVisible = true;
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        private async void BtnGoBacktoPage5_Clicked(object sender, EventArgs e)
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

                        fafPage6.IsVisible = false;
                        fafPage5.IsVisible = true;
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        private async void BtnGoBacktoPage6_Clicked(object sender, EventArgs e)
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

                        fafPage7.IsVisible = false;
                        fafPage6.IsVisible = true;
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        private async void btnGotoPage2_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entRetailerCode.Text) || codePicker.SelectedIndex < 0 || string.IsNullOrEmpty(entLandmark.Text) || string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) ||
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
                        }
                        else
                        {
                            await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                            await Navigation.PopToRootAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                    }
                }
            }
        }

        private async void btnGotoPage3_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) || string.IsNullOrEmpty(entCountry.Text) || string.IsNullOrEmpty(entTownCode.Text) || string.IsNullOrEmpty(entProvinceCode.Text))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Got it");

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

                            fafPage2.IsVisible = false;
                            fafPage3.IsVisible = true;
                        }
                        else
                        {
                            await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                            await Navigation.PopToRootAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                    }
                }
            }         
        }

        private async void BtnGotoPage4_Clicked(object sender, EventArgs e)
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
                        fafPage4.IsVisible = true;
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        private async void BtnGotoPage5_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entLocation.Text) || string.IsNullOrEmpty(entOnsiteLocation.Text))
            {
                if (string.IsNullOrEmpty(entLocation.Text) || string.IsNullOrEmpty(entOnsiteLocation.Text))
                {
                    await DisplayAlert("Form Required", "Please wait for the device to capture your location", "Got it");
                }
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        fafPage4.IsVisible = false;
                        fafPage5.IsVisible = true;
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        private async void BtnGotoPage6_Clicked(object sender, EventArgs e)
        {
            if ((swRekorida.IsToggled == false && swMerchandizing.IsToggled == false && swTradeCheck.IsToggled == false && swOthers.IsToggled == false) || (swOthers.IsToggled == true && string.IsNullOrEmpty(entOthers.Text)))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Got it");

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
            }
            else
            {
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        fafPage5.IsVisible = false;
                        fafPage6.IsVisible = true;
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        private async void BtnGotoPage7_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entPhoto1Url.Text) || string.IsNullOrEmpty(entPhoto2Url.Text) || string.IsNullOrEmpty(entPhoto3Url.Text))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Got it");

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
                try
                {
                    if (DateTime.Now >= DateTime.Parse(Preferences.Get("appdatetime", String.Empty, "private_prefs")))
                    {
                        Preferences.Set("appdatetime", DateTime.Now.ToString(), "private_prefs");

                        fafPage6.IsVisible = false;
                        fafPage7.IsVisible = true;
                        photovideovalidator.IsVisible = false;
                    }
                    else
                    {
                        await DisplayAlert("Application Error", "It appears you change the time/date of your phone. Please restore the correct time/date", "Got it");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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
                                fafPage7.IsVisible = false;
                                sendstatusform.IsVisible = true;

                                sendStatus.Text = "Checking internet connection";

                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    Send_online();
                                }
                                else
                                {
                                    Send_offline();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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

                        await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new AddRetailerOutlet(host, database, contact, ipaddress, selected))
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
                    Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(entRetailer.Text))
                {
                    entRetailerCode.Text = null;
                    entEmployeeNumber.Text = null;
                    entStreet.Text = null;
                    entBarangay.Text = null;
                    provinceSearch.Text = null;
                    entProvinceCode.Text = null;
                    entTownCode.Text = null;
                    townPicker.SelectedIndex = -1;
                    entDistrict.Text = null;
                    
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
               await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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

        public async void province_search()
        {
            try
            {
                var keyword = provinceSearch.Text;

                if (!string.IsNullOrEmpty(keyword))
                {
                    lstProvince.IsVisible = true;

                    var db = DependencyService.Get<ISQLiteDB>();
                    var conn = db.GetConnection();

                    var sql = "SELECT * FROM tblProvince WHERE Province LIKE '%"+keyword+"%' AND Deleted != '1' ORDER BY Province LIMIT 3";
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
               await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getTown = conn.QueryAsync<TownTable>("SELECT * FROM tblTown WHERE ProvinceID=? AND Deleted != '1'", item.ProvinceID).Result;

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
               await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public class ServerMessage
        {
            public string Message { get; set; }
        }

        public async void Send_online()
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

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
            var actlocation = entOnsiteLocation.Text;
            var date = dpDate.Text;
            var startTime = tpTime.Text;
            var endTime = DateTime.Now.ToString("HH:mm:ss");
            var photo1url = entPhoto1Url.Text;
            var photo2url = entPhoto2Url.Text;
            var photo3url = entPhoto3Url.Text;
            var videourl = entVideoUrl.Text;
            var otherconcern = entOthers.Text;
            var remarks = entRemarks.Text;
            string rekorida;
            string merchandizing;
            string tradecheck;
            string others;
            var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var getUsername = conn.QueryAsync<UserTable>("SELECT UserID FROM tblUser WHERE ContactID = ? AND Deleted != '1'", contact);
            var crresult = getUsername.Result[0];
            var username = crresult.UserID;
            var recordlog = "AB :" + username + "->" + contact + " " + current_datetime;
            var editrecordlog = "EB :" + username + "->" + contact + " " + current_datetime;

            if (swRekorida.IsToggled == true)
            {
                rekorida = "ACT00001";
            }
            else
            {
                rekorida = "";
            }

            if (swMerchandizing.IsToggled == true)
            {
                merchandizing = "ACT00002";
            }
            else
            {
                merchandizing = "";
            }

            if (swTradeCheck.IsToggled == true)
            {
                tradecheck = "ACT00003";
            }
            else
            {
                tradecheck = "";
            }

            if (swOthers.IsToggled == true)
            {
                others = "ACT00004";
            }
            else
            {
                others = "";
            }

            try
            {
                sendStatus.Text = "Sending field activity to server";

                
                
                string pathfile = "sync-caf-directly-api.php";

                var url = "http://" + ipaddress + ":" + Constants.port + "/" + Constants.apifolder + "/api/" + pathfile;
                string contentType = "application/json";
                JObject json = new JObject
                {
                    { "Host", host },
                    { "Database", database },
                    { "CAFNo", caf },
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
                    { "MobilePhoto1", photo1url },
                    { "MobilePhoto2", photo2url },
                    { "MobilePhoto3", photo3url },
                    { "MobileVideo", videourl },
                    { "GPSCoordinates", actlocation },
                    { "Rekorida", rekorida },
                    { "Merchandizing", merchandizing },
                    { "TradeCheck", tradecheck },
                    { "Others", others },
                    { "OtherConcern", otherconcern },
                    { "Remarks", remarks },
                    { "RecordLog", recordlog },
                    { "LastUpdated", DateTime.Parse(current_datetime) }
                };

                HttpClient client = new HttpClient();
                var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content))
                    {
                        var dataresult = JsonConvert.DeserializeObject<List<ServerMessage>>(content, settings);

                        var dataitem = dataresult[0];
                        var datamessage = dataitem.Message;

                        if (datamessage.Equals("Inserted"))
                        {
                            sendStatus.Text = "Sending field activity photo 1 to server";

                            string path1file = "sync-caf-media-path-1-client-update-api.php";

                            var path1link = "http://" + ipaddress + ":" + Constants.port + "/" + Constants.apifolder + "/api/" + path1file;
                            string ph1contentType = "application/json";

                            JObject path1json;
                            bool path1doesExist = File.Exists(photo1url);

                            if (!path1doesExist || string.IsNullOrEmpty(photo1url))
                            {
                                path1json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "MediaID", caf},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path1json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "MediaID", caf},
                                    { "Path", File.ReadAllBytes(photo1url)}
                                };
                            }

                            HttpClient ph1client = new HttpClient();
                            var ph1response = await ph1client.PostAsync(path1link, new StringContent(path1json.ToString(), Encoding.UTF8, ph1contentType));

                            if (ph1response.IsSuccessStatusCode)
                            {
                                var ph1content = await ph1response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(ph1content))
                                {
                                    var ph1result = JsonConvert.DeserializeObject<List<ServerMessage>>(ph1content, settings);

                                    var ph1item = ph1result[0];
                                    var ph1message = ph1item.Message;

                                    if (ph1message.Equals("Inserted"))
                                    {
                                        sendStatus.Text = "Sending field activity photo 2 to server";

                                        string path2file = "sync-caf-media-path-2-client-update-api.php";

                                        var path2link = "http://" + ipaddress + ":" + Constants.port + "/" + Constants.apifolder + "/api/" + path2file;
                                        string ph2contentType = "application/json";

                                        JObject path2json;
                                        bool path2doesExist = File.Exists(photo2url);

                                        if (!path2doesExist || string.IsNullOrEmpty(photo2url))
                                        {
                                            path2json = new JObject
                                            {
                                                { "Host", host },
                                                { "Database", database },
                                                { "MediaID", caf},
                                                { "Path", ""}
                                            };
                                        }
                                        else
                                        {
                                            path2json = new JObject
                                            {
                                                { "Host", host },
                                                { "Database", database },
                                                { "MediaID", caf},
                                                { "Path", File.ReadAllBytes(photo2url)}
                                            };
                                        }

                                        HttpClient ph2client = new HttpClient();
                                        var ph2response = await ph2client.PostAsync(path2link, new StringContent(path2json.ToString(), Encoding.UTF8, ph2contentType));

                                        if (ph2response.IsSuccessStatusCode)
                                        {
                                            var ph2content = await ph2response.Content.ReadAsStringAsync();

                                            if (!string.IsNullOrEmpty(ph2content))
                                            {
                                                var ph2result = JsonConvert.DeserializeObject<List<ServerMessage>>(ph2content, settings);

                                                var ph2item = ph2result[0];
                                                var ph2message = ph2item.Message;

                                                if (ph2message.Equals("Inserted"))
                                                {
                                                    sendStatus.Text = "Sending field activity photo 3 to server";

                                                    string path3file = "sync-caf-media-path-3-client-update-api.php";

                                                    var path3link = "http://" + ipaddress + ":" + Constants.port + "/" + Constants.apifolder + "/api/" + path3file;
                                                    string ph3contentType = "application/json";
                                                    
                                                    JObject path3json;
                                                    bool path3doesExist = File.Exists(photo3url);

                                                    if (!path3doesExist || string.IsNullOrEmpty(photo3url))
                                                    {
                                                        path3json = new JObject
                                                        {
                                                            { "Host", host },
                                                            { "Database", database },
                                                            { "MediaID", caf},
                                                            { "Path", ""}
                                                        };
                                                    }
                                                    else
                                                    {
                                                        path3json = new JObject
                                                        {
                                                            { "Host", host },
                                                            { "Database", database },
                                                            { "MediaID", caf},
                                                            { "Path", File.ReadAllBytes(photo3url)}
                                                        };
                                                    }

                                                    HttpClient ph3client = new HttpClient();
                                                    var ph3response = await ph3client.PostAsync(path3link, new StringContent(path3json.ToString(), Encoding.UTF8, ph3contentType));

                                                    if (ph3response.IsSuccessStatusCode)
                                                    {
                                                        var ph3content = await ph3response.Content.ReadAsStringAsync();

                                                        if (!string.IsNullOrEmpty(ph3content))
                                                        {
                                                            var ph3result = JsonConvert.DeserializeObject<List<ServerMessage>>(ph3content, settings);

                                                            var ph3item = ph3result[0];
                                                            var ph3message = ph3item.Message;

                                                            if (ph3message.Equals("Inserted"))
                                                            {
                                                                if (!string.IsNullOrEmpty(videourl))
                                                                {
                                                                    sendStatus.Text = "Sending field activity video to server";

                                                                    string path4file = "sync-caf-media-path-4-client-update-api.php";

                                                                    var path4link = "http://" + ipaddress + ":" + Constants.port + "/" + Constants.apifolder + "/api/" + path4file;
                                                                    string vidcontentType = "application/json";

                                                                    JObject path4json;
                                                                    bool path4doesExist = File.Exists(videourl);

                                                                    if (!path4doesExist || string.IsNullOrEmpty(photo3url))
                                                                    {
                                                                        path4json = new JObject
                                                                        {
                                                                            { "Host", host },
                                                                            { "Database", database },
                                                                            { "MediaID", caf},
                                                                            { "Path", ""}
                                                                        };
                                                                    }
                                                                    else
                                                                    {
                                                                        path4json = new JObject
                                                                        {
                                                                            { "Host", host },
                                                                            { "Database", database },
                                                                            { "MediaID", caf},
                                                                            { "Path", File.ReadAllBytes(videourl)}
                                                                        };
                                                                    }

                                                                    HttpClient vidclient = new HttpClient();
                                                                    var vidresponse = await vidclient.PostAsync(path4link, new StringContent(path4json.ToString(), Encoding.UTF8, vidcontentType));

                                                                    if (vidresponse.IsSuccessStatusCode)
                                                                    {
                                                                        var vidcontent = await vidresponse.Content.ReadAsStringAsync();

                                                                        if (!string.IsNullOrEmpty(vidcontent))
                                                                        {
                                                                            var vidresult = JsonConvert.DeserializeObject<List<ServerMessage>>(vidcontent, settings);

                                                                            var viditem = vidresult[0];
                                                                            var vidmessage = viditem.Message;

                                                                            if (vidmessage.Equals("Inserted"))
                                                                            {
                                                                                sendStatus.Text = "Saving field activity to the device";

                                                                                await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET PresStreet = ?, PresBarangay = ?, PresTown = ?, PresProvince = ?, PresCountry = ?, PresDistrict= ?, Landmark = ?, Telephone1 = ?, Telephone2 = ?, Mobile = ?, Email = ?, GPSCoordinates = ?, RecordLog = ?, LastUpdated = ?, LastSync = ? WHERE RetailerCode = ?", street, barangay, town, province, country, district, landmark, telephone1, telephone2, mobile, email, location, editrecordlog, DateTime.Parse(current_datetime), DateTime.Parse(current_datetime), retailerCode);

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
                                                                                    GPSCoordinates = actlocation,
                                                                                    Remarks = remarks,
                                                                                    OtherConcern = otherconcern,
                                                                                    RecordLog = recordlog,
                                                                                    LastSync = DateTime.Parse(current_datetime),
                                                                                    LastUpdated = DateTime.Parse(current_datetime)
                                                                                };

                                                                                await conn.InsertAsync(caf_insert);

                                                                                if (swRekorida.IsToggled == true)
                                                                                {
                                                                                    var rekorida_insert = new ActivityTable
                                                                                    {
                                                                                        CAFNo = caf,
                                                                                        ContactID = employeeNumber,
                                                                                        ActivityID = "ACT00001",
                                                                                        RecordLog = recordlog,
                                                                                        LastSync = DateTime.Parse(current_datetime),
                                                                                        LastUpdated = DateTime.Parse(current_datetime)
                                                                                    };

                                                                                    await conn.InsertAsync(rekorida_insert);
                                                                                }

                                                                                if (swMerchandizing.IsToggled == true)
                                                                                {
                                                                                    var merchandizing_insert = new ActivityTable
                                                                                    {
                                                                                        CAFNo = caf,
                                                                                        ContactID = employeeNumber,
                                                                                        ActivityID = "ACT00002",
                                                                                        RecordLog = recordlog,
                                                                                        LastSync = DateTime.Parse(current_datetime),
                                                                                        LastUpdated = DateTime.Parse(current_datetime)
                                                                                    };

                                                                                    await conn.InsertAsync(merchandizing_insert);
                                                                                }

                                                                                if (swTradeCheck.IsToggled == true)
                                                                                {
                                                                                    var trade_check_insert = new ActivityTable
                                                                                    {
                                                                                        CAFNo = caf,
                                                                                        ContactID = employeeNumber,
                                                                                        ActivityID = "ACT00003",
                                                                                        RecordLog = recordlog,
                                                                                        LastSync = DateTime.Parse(current_datetime),
                                                                                        LastUpdated = DateTime.Parse(current_datetime)
                                                                                    };

                                                                                    await conn.InsertAsync(trade_check_insert);
                                                                                }

                                                                                if (swOthers.IsToggled == true)
                                                                                {
                                                                                    var others_insert = new ActivityTable
                                                                                    {
                                                                                        CAFNo = caf,
                                                                                        ContactID = employeeNumber,
                                                                                        ActivityID = "ACT00004",
                                                                                        RecordLog = recordlog,
                                                                                        LastSync = DateTime.Parse(current_datetime),
                                                                                        LastUpdated = DateTime.Parse(current_datetime)
                                                                                    };

                                                                                    await conn.InsertAsync(others_insert);
                                                                                }

                                                                                var logType = "App Log";
                                                                                var log = "Sent caf to the server (<b>" + caf + "/b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                                                                int logdeleted = 0;

                                                                                Save_Logs(contact, logType, log, database, logdeleted);
                                                                                await DisplayAlert("Data Sent", "Your activity has been sent to the server", "Got it");

                                                                                await Application.Current.MainPage.Navigation.PopAsync();
                                                                            }
                                                                            else
                                                                            {
                                                                                sendStatus.Text = "Syncing failed. Failed to send the data.\n\n Error: " + vidmessage;
                                                                                Send_offline();
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            sendStatus.Text = "Syncing failed. Failed to send the data.";
                                                                            Send_offline();
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Send_offline();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    sendStatus.Text = "Saving field activity to the device";

                                                                    await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET PresStreet = ?, PresBarangay = ?, PresTown = ?, PresProvince = ?, PresCountry = ?, PresDistrict= ?, Landmark = ?, Telephone1 = ?, Telephone2 = ?, Mobile = ?, Email = ?, GPSCoordinates = ?, RecordLog = ?, LastUpdated = ?, LastSync = ? WHERE RetailerCode = ?", street, barangay, town, province, country, district, landmark, telephone1, telephone2, mobile, email, location, editrecordlog, DateTime.Parse(current_datetime), DateTime.Parse(current_datetime), retailerCode);

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
                                                                        GPSCoordinates = actlocation,
                                                                        Remarks = remarks,
                                                                        OtherConcern = otherconcern,
                                                                        RecordLog = recordlog,
                                                                        LastSync = DateTime.Parse(current_datetime),
                                                                        LastUpdated = DateTime.Parse(current_datetime)
                                                                    };

                                                                    await conn.InsertAsync(caf_insert);

                                                                    if (swRekorida.IsToggled == true)
                                                                    {
                                                                        var rekorida_insert = new ActivityTable
                                                                        {
                                                                            CAFNo = caf,
                                                                            ContactID = employeeNumber,
                                                                            ActivityID = "ACT00001",
                                                                            RecordLog = recordlog,
                                                                            LastSync = DateTime.Parse(current_datetime),
                                                                            LastUpdated = DateTime.Parse(current_datetime)
                                                                        };

                                                                        await conn.InsertAsync(rekorida_insert);
                                                                    }

                                                                    if (swMerchandizing.IsToggled == true)
                                                                    {
                                                                        var merchandizing_insert = new ActivityTable
                                                                        {
                                                                            CAFNo = caf,
                                                                            ContactID = employeeNumber,
                                                                            ActivityID = "ACT00002",
                                                                            RecordLog = recordlog,
                                                                            LastSync = DateTime.Parse(current_datetime),
                                                                            LastUpdated = DateTime.Parse(current_datetime)
                                                                        };

                                                                        await conn.InsertAsync(merchandizing_insert);
                                                                    }

                                                                    if (swTradeCheck.IsToggled == true)
                                                                    {
                                                                        var trade_check_insert = new ActivityTable
                                                                        {
                                                                            CAFNo = caf,
                                                                            ContactID = employeeNumber,
                                                                            ActivityID = "ACT00003",
                                                                            RecordLog = recordlog,
                                                                            LastSync = DateTime.Parse(current_datetime),
                                                                            LastUpdated = DateTime.Parse(current_datetime)
                                                                        };

                                                                        await conn.InsertAsync(trade_check_insert);
                                                                    }

                                                                    if (swOthers.IsToggled == true)
                                                                    {
                                                                        var others_insert = new ActivityTable
                                                                        {
                                                                            CAFNo = caf,
                                                                            ContactID = employeeNumber,
                                                                            ActivityID = "ACT00004",
                                                                            RecordLog = recordlog,
                                                                            LastSync = DateTime.Parse(current_datetime),
                                                                            LastUpdated = DateTime.Parse(current_datetime)
                                                                        };

                                                                        await conn.InsertAsync(others_insert);
                                                                    }

                                                                    Analytics.TrackEvent("Sent Field Activity Form");
                                                                    sendStatus.Text = "Sending user logs to server";

                                                                    var logType = "App Log";
                                                                    var log = "Sent caf to the server (<b>"+ caf + "/b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                                                    int logdeleted = 0;

                                                                    Save_Logs(contact, logType, log, database, logdeleted);

                                                                    await DisplayAlert("Data Sent", "Your activity has been sent to the server", "Got it");
                                                                    await Application.Current.MainPage.Navigation.PopAsync();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                sendStatus.Text = "Syncing failed. Failed to send the data.\n\n Error: " + ph3message;
                                                                Send_offline();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sendStatus.Text = "Syncing failed. Failed to send the data.";
                                                            Send_offline();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Send_offline();
                                                    }
                                                }
                                                else
                                                {
                                                    sendStatus.Text = "Syncing failed. Failed to send the data.\n\n Error: " + ph2message;
                                                    Send_offline();
                                                }
                                            }
                                            else
                                            {
                                                sendStatus.Text = "Syncing failed. Failed to send the data.";
                                                Send_offline();
                                            }
                                        }
                                        else
                                        {
                                            Send_offline();
                                        }
                                    }
                                    else
                                    {
                                        sendStatus.Text = "Syncing failed. Failed to send the data.\n\n Error: " + ph1message;
                                        Send_offline();
                                    }
                                }
                                else
                                {
                                    sendStatus.Text = "Syncing failed. Failed to send the data.";
                                    Send_offline();
                                }
                            }
                            else
                            {
                                Send_offline();
                            }
                        }
                        else
                        {
                            sendStatus.Text = "Syncing failed. Failed to send the data.\n\n Error: " + datamessage;
                            Send_offline();
                        }
                    }
                    else
                    {
                        sendStatus.Text = "Syncing failed. Failed to send the data.";
                        Send_offline();
                    }
                }
                else
                {
                    Send_offline();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                Send_offline();
            }
        }

        public async void Send_offline()
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

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
            var actlocation = entOnsiteLocation.Text;
            var date = dpDate.Text;
            var startTime = tpTime.Text;
            var endTime = DateTime.Now.ToString("HH:mm:ss");
            var photo1url = entPhoto1Url.Text;
            var photo2url = entPhoto2Url.Text;
            var photo3url = entPhoto3Url.Text;
            var videourl = entVideoUrl.Text;
            var otherconcern = entOthers.Text;
            var remarks = entRemarks.Text;
            var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var getUsername = conn.QueryAsync<UserTable>("SELECT UserID FROM tblUser WHERE ContactID = ? AND Deleted != '1'", contact);
            var crresult = getUsername.Result[0];
            var username = crresult.UserID;
            var recordlog = "AB :" + username + "->" + contact + " " + current_datetime;
            var editrecordlog = "EB :" + username + "->" + contact + " " + current_datetime;

            await conn.QueryAsync<RetailerGroupTable>("UPDATE tblRetailerGroup SET PresStreet = ?, PresBarangay = ?, PresTown = ?, PresProvince = ?, PresCountry = ?, PresDistrict= ?, Landmark = ?, Telephone1 = ?, Telephone2 = ?, Mobile = ?, Email = ?, GPSCoordinates = ?, RecordLog = ?, LastUpdated = ? WHERE RetailerCode = ?", street, barangay, town, province, country, district, landmark, telephone1, telephone2, mobile, email, location, editrecordlog, DateTime.Parse(current_datetime), retailerCode);

            sendStatus.Text = "Saving field activity to the device";

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
                GPSCoordinates = actlocation,
                Remarks = remarks,
                OtherConcern = otherconcern,
                RecordLog = recordlog,
                LastUpdated = DateTime.Parse(current_datetime)
            };

            await conn.InsertAsync(caf_insert);

            if (swRekorida.IsToggled == true)
            {
                var rekorida_insert = new ActivityTable
                {
                    CAFNo = caf,
                    ContactID = employeeNumber,
                    ActivityID = "ACT00001",
                    RecordLog = recordlog,
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await conn.InsertAsync(rekorida_insert);
            }

            if (swMerchandizing.IsToggled == true)
            {
                var merchandizing_insert = new ActivityTable
                {
                    CAFNo = caf,
                    ContactID = employeeNumber,
                    ActivityID = "ACT00002",
                    RecordLog = recordlog,
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await conn.InsertAsync(merchandizing_insert);
            }

            if (swTradeCheck.IsToggled == true)
            {
                var trade_check_insert = new ActivityTable
                {
                    CAFNo = caf,
                    ContactID = employeeNumber,
                    ActivityID = "ACT00003",
                    RecordLog = recordlog,
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await conn.InsertAsync(trade_check_insert);
            }

            if (swOthers.IsToggled == true)
            {
                var others_insert = new ActivityTable
                {
                    CAFNo = caf,
                    ContactID = employeeNumber,
                    ActivityID = "ACT00004",
                    RecordLog = recordlog,
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                await conn.InsertAsync(others_insert);
            }

            var logtype = "Mobile Log";
            var log = "Added field activity(<b>" + caf + "</b>)" + "Version: <b>" + Constants.appversion + "</b> Device ID: <b>" + CrossDeviceInfo.Current.Id + "</b>";
            int deleted = 0;

            sendStatus.Text = "Saving user logs to the device";

            var logs_insert = new UserLogsTable
            {
                ContactID = contact,
                LogType = logtype,
                Log = log,
                LogDate = DateTime.Parse(current_datetime),
                DatabaseName = database,
                Deleted = deleted,
                LastUpdated = DateTime.Parse(current_datetime)
            };

            await conn.InsertOrReplaceAsync(logs_insert);

            Analytics.TrackEvent("Sent Field Activity Form");

            await DisplayAlert("Offline Send", "Your activity has been saved offline. Connect to the server to sync your activity", "Got it");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async void Send_email()
        {
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
            var endTime = DateTime.Now.ToString("HH:mm:ss");
            var photo1url = entPhoto1Url.Text;
            var photo2url = entPhoto2Url.Text;
            var photo3url = entPhoto3Url.Text;
            var videourl = entVideoUrl.Text;
            var otherconcern = entOthers.Text;
            var remarks = entRemarks.Text;
            string rekorida;
            string merchandizing;
            string tradecheck;
            string others;
            var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var recipients = entrecipient.Text;
            var emailMessenger = CrossMessaging.Current.EmailMessenger;

            if (swRekorida.IsToggled == true)
            {
                rekorida = "True";
            }
            else
            {
                rekorida = "False";
            }

            if (swMerchandizing.IsToggled == true)
            {
                merchandizing = "True";
            }
            else
            {
                merchandizing = "False";
            }

            if (swTradeCheck.IsToggled == true)
            {
                tradecheck = "True";
            }
            else
            {
                tradecheck = "False";
            }

            if (swOthers.IsToggled == true)
            {
                others = "True";
            }
            else
            {
                others = "False";
            }

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
                            { "LastSync", DateTime.Parse(current_datetime) },
                            { "LastUpdated", DateTime.Parse(current_datetime) }
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
                                LastSync = DateTime.Parse(current_datetime),
                                LastUpdated = DateTime.Parse(current_datetime)
                            };

                            await conn.InsertOrReplaceAsync(email_update);
                        }
                        else
                        {
                            var insert_email = new UserEmailTable
                            {
                                ContactID = employeeNumber,
                                Email = recipients,
                                LastSync = DateTime.Parse(current_datetime),
                                LastUpdated = DateTime.Parse(current_datetime)
                            };

                            await conn.InsertOrReplaceAsync(insert_email);
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                    }
                }
                else
                {
                    try
                    {
                        var db = DependencyService.Get<ISQLiteDB>();
                        var conn = db.GetConnection();

                        var getCode = conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID = ?", employeeNumber);
                        var resultCount = getCode.Result.Count;
                        if (resultCount > 0)
                        {
                            var email_update = new UserEmailTable
                            {
                                ContactID = employeeNumber,
                                Email = recipients,
                                LastSync = DateTime.Parse(current_datetime),
                                LastUpdated = DateTime.Parse(current_datetime)
                            };

                            await conn.InsertOrReplaceAsync(email_update);
                        }
                        else
                        {
                            var insert_email = new UserEmailTable
                            {
                                ContactID = employeeNumber,
                                Email = recipients,
                                LastSync = DateTime.Parse(current_datetime),
                                LastUpdated = DateTime.Parse(current_datetime)
                            };

                            await conn.InsertOrReplaceAsync(insert_email);
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex); await DisplayAlert("App Error", ex.Message.ToString(), "ok");
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
                    await Navigation.PushAsync(new FieldActivityForm(host, database, contact, ipaddress));
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
                    await Navigation.PushAsync(new FieldActivityForm(host, database, contact, ipaddress));
                }
            }
        }

        public async void Save_Logs(string contactID, string logType, string log, string database, int deleted)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var logs_insert = new UserLogsTable
            {
                ContactID = contactID,
                LogType = logType,
                Log = log,
                LogDate = DateTime.Parse(current_datetime),
                DatabaseName = database,
                Deleted = deleted,
                LastUpdated = DateTime.Parse(current_datetime)
            };

            await conn.InsertOrReplaceAsync(logs_insert);
        }
    }
}