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
using System.Net.Http;
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
                var getRecipient = Constants.conn.QueryAsync<UserEmailTable>("SELECT * FROM tblUserEmail WHERE ContactID=?", contact);
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
               await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                locator.DesiredAccuracy = 300;

                if (!locator.IsGeolocationAvailable)
                {
                    await DisplayAlert("GPS Error", "GPS location not available", "Ok");
                }
                else if (!locator.IsGeolocationEnabled)
                {
                    await DisplayAlert("GPS Error", "GPS location is not enabled", "Ok");
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

                var getCode = Constants.conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=?", item.ContactID);
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
               await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
               await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

                    var getCode = Constants.conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode=? AND Deleted != '1'", code);
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
                        var getProvince = Constants.conn.QueryAsync<ProvinceTable>(getProvincesql);
                        var provinceresultCount = getProvince.Result.Count;

                        if (provinceresultCount > 0)
                        {
                            var prvresult = getProvince.Result[0];
                            provinceSearch.Text = prvresult.Province;
                            lstProvince.IsVisible = false;
                        }

                        //var getProvince = Constants.conn.QueryAsync<ProvinceTable>("SELECT * FROM tblProvince").Result;

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
                            var getTown = Constants.conn.QueryAsync<TownTable>("SELECT * FROM tblTown WHERE ProvinceID=?", result.PresProvince).Result;

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
               await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void btnCamera4_Clicked(object sender, EventArgs e)
        {
            var cafNo = entCafNo.Text;
            TimeSpan time = new TimeSpan(0, 0, 0, 20, 0);

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
                Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
                }
            }
        }

        private async void btnGotoPage2_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entRetailerCode.Text) || codePicker.SelectedIndex < 0 || string.IsNullOrEmpty(entLandmark.Text) || string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) ||
                 string.IsNullOrEmpty(entCountry.Text) || string.IsNullOrEmpty(entTownCode.Text) || string.IsNullOrEmpty(entProvinceCode.Text))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Ok");

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
                           await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                            await Navigation.PopToRootAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
                    }
                }
            }
        }

        private async void btnGotoPage3_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) || string.IsNullOrEmpty(entCountry.Text) || string.IsNullOrEmpty(entTownCode.Text) || string.IsNullOrEmpty(entProvinceCode.Text))
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
                           await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                            await Navigation.PopToRootAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
                }
            }
        }

        private async void BtnGotoPage5_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entLocation.Text) || string.IsNullOrEmpty(entOnsiteLocation.Text))
            {
                if (string.IsNullOrEmpty(entLocation.Text) || string.IsNullOrEmpty(entOnsiteLocation.Text))
                {
                    await DisplayAlert("Form Required", "Please wait for the device to capture your location", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
                }
            }
        }

        private async void BtnGotoPage6_Clicked(object sender, EventArgs e)
        {
            if ((swRekorida.IsToggled == false && swMerchandizing.IsToggled == false && swTradeCheck.IsToggled == false && swOthers.IsToggled == false) || (swOthers.IsToggled == true && string.IsNullOrEmpty(entOthers.Text)))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Ok");

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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
                }
            }
        }

        private async void BtnGotoPage7_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entPhoto1Url.Text) || string.IsNullOrEmpty(entPhoto2Url.Text) || string.IsNullOrEmpty(entPhoto3Url.Text))
            {
                await DisplayAlert("Form Required", "Please fill-up the required field", "Ok");

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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

                                var getUsername = Constants.conn.QueryAsync<UserTable>("SELECT UserID FROM tblUser WHERE ContactID = ? AND Deleted != '1'", contact);
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

                                fafPage7.IsVisible = false;
                                sendstatusform.IsVisible = true;

                                sendStatus.Text = "Checking internet connection";

                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    await App.TodoManager.SendCAFDirectly(host, database, ipaddress, contact, SyncStatus, caf, retailerCode, employeeNumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, startTime, endTime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                                    await App.TodoManager.SendCAFMedia1Directly(host, database, ipaddress, contact, SyncStatus, caf, retailerCode, employeeNumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, startTime, endTime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                                    await App.TodoManager.SendCAFMedia2Directly(host, database, ipaddress, contact, SyncStatus, caf, retailerCode, employeeNumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, startTime, endTime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                                    await App.TodoManager.SendCAFMedia3Directly(host, database, ipaddress, contact, SyncStatus, caf, retailerCode, employeeNumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, startTime, endTime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                                    await App.TodoManager.SendCAFMedia4Directly(host, database, ipaddress, contact, SyncStatus, caf, retailerCode, employeeNumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, startTime, endTime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog, rekorida, merchandizing, tradecheck, others);
                                    await App.TodoManager.OnSendComplete(host, database, ipaddress, contact);
                                    Send_email();
                                }
                                else
                                {
                                    await App.TodoManager.SaveCAFToLocalDatabaseFailed(host, database, ipaddress, contact, SyncStatus, caf, retailerCode, employeeNumber, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, date, startTime, startTime, photo1url, photo2url, photo3url, videourl, actlocation, otherconcern, remarks, recordlog);
                                    await App.TodoManager.SaveRetailerOutletToLocalDatabaseFailed(host, database, ipaddress, contact, SyncStatus, retailerCode, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, location, recordlog);
                                    await App.TodoManager.SaveCAFActivityToLocalDatabaseFailed(host, database, ipaddress, contact, SyncStatus, caf, employeeNumber, recordlog, rekorida, merchandizing, tradecheck, others);
                                    await App.TodoManager.OnSendComplete(host, database, ipaddress, contact);
                                    Send_email();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                       await DisplayAlert("Application Error", "It appears you change the time/date of your phone. You will be logged out. Please restore the correct time/date", "Ok");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex); await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

                    var getCode = Constants.conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE ContactID=?", entRetailer.Text);
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
               await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

                    var sql = "SELECT * FROM tblProvince WHERE Province LIKE '%"+keyword+"%' AND Deleted != '1' ORDER BY Province LIMIT 3";
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

        public void Send_email()
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
                    if (!string.IsNullOrEmpty(videourl))
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
            }
        }

        private void SyncStatus(string status)
        {
            Device.BeginInvokeOnMainThread(() => {
                sendStatus.Text = status;
            });
        }
    }
}