using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
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

        public AddProspectRetailer (string host, string database, string contact, string ipaddress)
		{
			InitializeComponent ();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            
            SetTempId();
            Init();
        }

        void Init()
        {
            dpDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            tpTime.Text = DateTime.Now.ToString("HH:mm:ss");
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
            if (string.IsNullOrEmpty(entFirstName.Text) || string.IsNullOrEmpty(entMiddleName.Text) || string.IsNullOrEmpty(entLastName.Text))
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

        private async void btnBackPage1_Clicked(object sender, EventArgs e)
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
            if (string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) || string.IsNullOrEmpty(entTownCode.Text) || string.IsNullOrEmpty(entProvinceCode.Text) ||
                string.IsNullOrEmpty(entCountry.Text) || string.IsNullOrEmpty(entRemarks.Text))
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

                if (string.IsNullOrEmpty(entRemarks.Text))
                {
                    remarksvalidator.IsVisible = true;
                    RemarksFrame.BorderColor = Color.FromHex("#e74c3c");
                }
                else
                {
                    remarksvalidator.IsVisible = false;
                    RemarksFrame.BorderColor = Color.FromHex("#e8eaed");
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

                            acPage2.IsVisible = false;
                            acPage3.IsVisible = true;
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

                       acPage2.IsVisible = true;
                       acPage3.IsVisible = false;
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
                            await DisplayAlert("Form Required", "Please fill-up the required field", "Got it");
                            photovideovalidator.IsVisible = true;
                        }
                        else
                        {
                            photovideovalidator.IsVisible = false;

                            var confirm = await DisplayAlert("Sending Confirmation", "Are you sure you want to send this form?", "Yes", "No");

                            if (confirm == true)
                            {
                                acPage4.IsVisible = false;
                                sendstatusform.IsVisible = true;

                                sendStatus.Text = "Checking internet connection";

                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    sendStatus.Text = "Checking connection to server";

                                    Ping ping = new Ping();
                                    PingReply pingresult = ping.Send(ipaddress, 2000);
                                    if (pingresult.Status.ToString() == "Success")
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

                if (file == null)
                    return;

                entVideoUrl.Text = file.Path;

                if (!string.IsNullOrEmpty(entVideoUrl.Text))
                {
                    btnCamera4.IsVisible = false;
                    vidthumb.IsVisible = false;
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

                    var sql = "SELECT * FROM tblProvince WHERE Province LIKE '%" + keyword + "%' AND Deleted != '1' ORDER BY Province LIMIT 3";
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

            var id = entTempID.Text;
            var firstName = entFirstName.Text;
            var middleName = entMiddleName.Text;
            var lastName = entLastName.Text;
            var fileas = firstName + " " + middleName + " " + lastName;
            var retailerType = "RT00004";
            var street = entStreet.Text;
            var barangay = entBarangay.Text;
            var town = entTownCode.Text;
            var province = entProvinceCode.Text;
            var district = entDistrict.Text;
            var country = entCountry.Text;
            var landmark = entLandmark.Text;
            var remarks = entRemarks.Text;
            var date = dpDate.Text;
            var startTime = tpTime.Text;
            var endTime = DateTime.Now.ToString("HH:mm:ss");
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
            int deleted = 0;
            var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var getUsername = conn.QueryAsync<UserTable>("SELECT UserID FROM tblUser WHERE ContactID = ? AND Deleted != '1'", contact);
            var crresult = getUsername.Result[0];
            var username = crresult.UserID;
            var recordlog = "AB :" + username + "->" + contact + " " + current_datetime;
            var editrecordlog = "EB :" + username + "->" + contact + " " + current_datetime;

            try
            {
                sendStatus.Text = "Sending prospect retailer to server";

                var port = "7777";
                var apifolder = "TBSApp";
                string pathfile = "sync-prospect-directly-api.php";

                var url = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + pathfile;
                string contentType = "application/json";
                JObject json = new JObject
                {
                    { "Host", host },
                    { "Database", database },
                    { "ContactID", id },
                    { "FirstName", firstName },
                    { "MiddleName", middleName },
                    { "LastName", lastName },
                    { "FileAs", fileas },
                    { "RetailerType", retailerType },
                    { "Street", street },
                    { "Barangay", barangay },
                    { "Town", town },
                    { "Province", province },
                    { "District", district },
                    { "Country", country },
                    { "Landmark", landmark },
                    { "Remarks", remarks },
                    { "RecordDate", DateTime.Parse(date) },
                    { "StartTime", DateTime.Parse(startTime) },
                    { "EndTime", DateTime.Parse(endTime) },
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
                    { "Supervisor", contact },
                    { "RecordLog", recordlog },
                    { "Deleted", deleted },
                    { "LastSync", current_datetime },
                    { "LastUpdated", current_datetime }
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
                            sendStatus.Text = "Sending prospect retailer photo 1 to server";

                            string path1file = "sync-contact-media-path-1-client-update-api.php";

                            var path1link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path1file;
                            string ph1contentType = "application/json";

                            JObject path1json;
                            bool path1doesExist = File.Exists(photo1url);

                            if (!path1doesExist || string.IsNullOrEmpty(photo1url))
                            {
                                path1json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "MediaID", id},
                                    { "Path", ""}
                                };
                            }
                            else
                            {
                                path1json = new JObject
                                {
                                    { "Host", host },
                                    { "Database", database },
                                    { "MediaID", id},
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
                                        sendStatus.Text = "Sending prospect retailer photo 2 to server";

                                        string path2file = "sync-contact-media-path-2-client-update-api.php";

                                        var path2link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path2file;
                                        string ph2contentType = "application/json";

                                        JObject path2json;
                                        bool path2doesExist = File.Exists(photo2url);

                                        if (!path2doesExist || string.IsNullOrEmpty(photo2url))
                                        {
                                            path2json = new JObject
                                            {
                                                { "Host", host },
                                                { "Database", database },
                                                { "MediaID", id},
                                                { "Path", ""}
                                            };
                                        }
                                        else
                                        {
                                            path2json = new JObject
                                            {
                                                { "Host", host },
                                                { "Database", database },
                                                { "MediaID", id},
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
                                                    string path3file = "sync-contact-media-path-3-client-update-api.php";

                                                    var path3link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path3file;
                                                    string ph3contentType = "application/json";

                                                    JObject path3json;
                                                    bool path3doesExist = File.Exists(photo3url);

                                                    if (!path3doesExist || string.IsNullOrEmpty(photo3url))
                                                    {
                                                        path3json = new JObject
                                                        {
                                                            { "Host", host },
                                                            { "Database", database },
                                                            { "MediaID", id},
                                                            { "Path", ""}
                                                        };
                                                    }
                                                    else
                                                    {
                                                        path3json = new JObject
                                                        {
                                                            { "Host", host },
                                                            { "Database", database },
                                                            { "MediaID", id},
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
                                                                    sendStatus.Text = "Sending prospect retailer video to server";

                                                                    string path4file = "sync-contact-media-path-4-client-update-api.php";

                                                                    var path4link = "http://" + ipaddress + ":" + port + "/" + apifolder + "/api/" + path4file;
                                                                    string vidcontentType = "application/json";

                                                                    JObject path4json;
                                                                    bool path4doesExist = File.Exists(videourl);

                                                                    if (!path4doesExist || string.IsNullOrEmpty(photo3url))
                                                                    {
                                                                        path4json = new JObject
                                                                        {
                                                                            { "Host", host },
                                                                            { "Database", database },
                                                                            { "MediaID", id},
                                                                            { "Path", ""}
                                                                        };
                                                                    }
                                                                    else
                                                                    {
                                                                        path4json = new JObject
                                                                        {
                                                                            { "Host", host },
                                                                            { "Database", database },
                                                                            { "MediaID", id},
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
                                                                                sendStatus.Text = "Saving prospect retailer to the device";

                                                                                var retailer = new ContactsTable
                                                                                {
                                                                                    ContactID = id,
                                                                                    FileAs = firstName + " " + lastName + " " + middleName,
                                                                                    FirstName = firstName,
                                                                                    MiddleName = middleName,
                                                                                    LastName = lastName,
                                                                                    RetailerType = retailerType,
                                                                                    PresStreet = street,
                                                                                    PresBarangay = barangay,
                                                                                    PresDistrict = district,
                                                                                    PresTown = town,
                                                                                    PresProvince = province,
                                                                                    PresCountry = country,
                                                                                    Landmark = landmark,
                                                                                    CustomerRemarks = remarks,
                                                                                    RecordDate = DateTime.Parse(date),
                                                                                    StartTime = DateTime.Parse(startTime),
                                                                                    EndTime = DateTime.Parse(endTime),
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
                                                                                    Supervisor = contact,
                                                                                    RecordLog = recordlog,
                                                                                    Deleted = deleted,
                                                                                    LastSync = DateTime.Parse(current_datetime),
                                                                                    LastUpdated = DateTime.Parse(current_datetime)
                                                                                };

                                                                                await conn.InsertAsync(retailer);

                                                                                var logType = "App Log";
                                                                                var log = "Sent prospect retailer to the server (<b>" + id + "/b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                                                                int logdeleted = 0;

                                                                                Save_Logs(contact, logType, log, database, logdeleted);

                                                                                await DisplayAlert("Data Sent", "Prospect retailer has been sent to the server", "Got it");
                                                                                await Application.Current.MainPage.Navigation.PopModalAsync();
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Send_offline();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    sendStatus.Text = "Saving prospect retailer to the device";

                                                                    var retailer = new ContactsTable
                                                                    {
                                                                        ContactID = id,
                                                                        FileAs = firstName + " " + lastName + " " + middleName,
                                                                        FirstName = firstName,
                                                                        MiddleName = middleName,
                                                                        LastName = lastName,
                                                                        RetailerType = retailerType,
                                                                        PresStreet = street,
                                                                        PresBarangay = barangay,
                                                                        PresDistrict = district,
                                                                        PresTown = town,
                                                                        PresProvince = province,
                                                                        PresCountry = country,
                                                                        Landmark = landmark,
                                                                        CustomerRemarks = remarks,
                                                                        RecordDate = DateTime.Parse(date),
                                                                        StartTime = DateTime.Parse(startTime),
                                                                        EndTime = DateTime.Parse(endTime),
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
                                                                        Supervisor = contact,
                                                                        RecordLog = recordlog,
                                                                        Deleted = deleted,
                                                                        LastSync = DateTime.Parse(current_datetime),
                                                                        LastUpdated = DateTime.Parse(current_datetime)
                                                                    };

                                                                    await conn.InsertAsync(retailer);

                                                                    Analytics.TrackEvent("Sent Prospect Retailer");
                                                                    var logType = "App Log";
                                                                    var log = "Sent prospect retailer to the server (<b>" + id + "/b>)  <br/>" + "Version: <b>" + Constants.appversion + "</b><br/> Device ID: <b>" + Constants.deviceID + "</b>";
                                                                    int logdeleted = 0;

                                                                    Save_Logs(contact, logType, log, database, logdeleted);

                                                                    await DisplayAlert("Data Sent", "Prospect retailer has been sent to the server", "Got it");
                                                                    await Application.Current.MainPage.Navigation.PopModalAsync();
                                                                }
                                                            }
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
                                            Send_offline();
                                        }

                                    }
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
                    Send_offline();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Send_offline();
            }
        }

        public async void Send_offline()
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var id = entTempID.Text;
                var firstName = entFirstName.Text;
                var middleName = entMiddleName.Text;
                var lastName = entLastName.Text;
                var fileas = firstName + " " + middleName + " " + lastName;
                var retailerType = "RT00004";
                var street = entStreet.Text;
                var barangay = entBarangay.Text;
                var town = entTownCode.Text;
                var province = entProvinceCode.Text;
                var district = entDistrict.Text;
                var country = entCountry.Text;
                var landmark = entLandmark.Text;
                var remarks = entRemarks.Text;
                var date = dpDate.Text;
                var startTime = tpTime.Text;
                var endTime = DateTime.Now.ToString("HH:mm:ss");
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

                var getUsername = conn.QueryAsync<UserTable>("SELECT UserID FROM tblUser WHERE ContactID = ? AND Deleted != '1'", contact);
                var crresult = getUsername.Result[0];
                var username = crresult.UserID;
                var recordlog = "AB :" + username + "->" + contact + " " + current_datetime;
                var editrecordlog = "EB :" + username + "->" + contact + " " + current_datetime;

                sendStatus.Text = "Saving prospect retailer to the device";

                var prospect_insert = new ContactsTable
                {
                    ContactID = id,
                    FileAs = fileas,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    RetailerType = retailerType,
                    PresStreet = street,
                    PresBarangay = barangay,
                    PresDistrict = district,
                    PresTown = town,
                    PresProvince = province,
                    PresCountry = country,
                    Landmark = landmark,
                    CustomerRemarks = remarks,
                    RecordDate = DateTime.Parse(date),
                    StartTime = DateTime.Parse(startTime),
                    EndTime = DateTime.Parse(endTime),
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
                    Supervisor = contact,
                    RecordLog = recordlog,
                    LastUpdated = DateTime.Parse(current_datetime)
                };

                sendStatus.Text = "Saving user logs to the device";

                await conn.InsertAsync(prospect_insert);

                var logtype = "Mobile Log";
                var log = "Added prospect retailer(<b>" + fileas + "</b>)" + "Version: <b>" + Constants.appversion + "</b> Device ID: <b>" + CrossDeviceInfo.Current.Id + "</b>";
                int deleted = 0;

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
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            Analytics.TrackEvent("Sent Prospect Retailer");
            await DisplayAlert("Offline Save", "Prospect retailer has been saved offline. Connect to the server to sync your data", "Got it");
            await Application.Current.MainPage.Navigation.PopModalAsync();
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

                        acPage4.IsVisible = true;
                        acPage3.IsVisible = false;
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

        private async void BtnBackPage3_Clicked(object sender, EventArgs e)
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

                        acPage3.IsVisible = true;
                        acPage4.IsVisible = false;
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

        private void EntRemarks_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entRemarks.Text))
            {
                remarksvalidator.IsVisible = true;
                RemarksFrame.BorderColor = Color.FromHex("#e74c3c");
            }
            else
            {
                remarksvalidator.IsVisible = false;
                RemarksFrame.BorderColor = Color.FromHex("#e8eaed");
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