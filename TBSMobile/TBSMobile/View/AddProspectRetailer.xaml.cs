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
using System.Net.Http;
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
                await DisplayAlert("Form Required", "Please fill-up the required field", "Ok");

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

        private async void btnGotoPage3_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entStreet.Text) || string.IsNullOrEmpty(entBarangay.Text) || string.IsNullOrEmpty(entTownCode.Text) || string.IsNullOrEmpty(entProvinceCode.Text) ||
                string.IsNullOrEmpty(entCountry.Text) || string.IsNullOrEmpty(entRemarks.Text))
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
                            await DisplayAlert("Form Required", "Please fill-up the required field", "Ok");
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
                                int deleted = 0;
                                var current_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                var getUsername = conn.QueryAsync<UserTable>("SELECT UserID FROM tblUser WHERE ContactID = ? AND Deleted != '1'", contact);
                                var crresult = getUsername.Result[0];
                                var username = crresult.UserID;
                                var recordlog = "AB :" + username + "->" + contact + " " + current_datetime;

                                sendStatus.Text = "Checking internet connection";

                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    await App.TodoManager.SendProspectRetailerDirectly(host, database, ipaddress, contact, SyncStatus, id, firstName, middleName, lastName, fileas, retailerType, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, startTime, endTime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                                    await App.TodoManager.SendProspectRetailerMedia1Directly(host, database, ipaddress, contact, SyncStatus, id, firstName, middleName, lastName, fileas, retailerType, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, startTime, endTime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                                    await App.TodoManager.SendProspectRetailerMedia2Directly(host, database, ipaddress, contact, SyncStatus, id, firstName, middleName, lastName, fileas, retailerType, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, startTime, endTime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                                    await App.TodoManager.SendProspectRetailerMedia3Directly(host, database, ipaddress, contact, SyncStatus, id, firstName, middleName, lastName, fileas, retailerType, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, startTime, endTime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                                    await App.TodoManager.SendProspectRetailerMedia4Directly(host, database, ipaddress, contact, SyncStatus, id, firstName, middleName, lastName, fileas, retailerType, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, startTime, endTime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                                    await App.TodoManager.OnSendCompleteModal(host, database, ipaddress, contact);
                                }
                                else
                                {
                                    await App.TodoManager.SaveProspectRetailerToLocalDatabaseFailed(host, database, ipaddress, contact, SyncStatus, id, firstName, middleName, lastName, fileas, retailerType, street, barangay, town, district, province, country, landmark, telephone1, telephone2, mobile, email, date, remarks, startTime, endTime, photo1url, photo2url, photo3url, videourl, employee, customer, deleted, recordlog);
                                    await App.TodoManager.OnSendCompleteModal(host, database, ipaddress, contact);
                                }
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
                await DisplayAlert("No Camera", "No Camera Available", "Ok");
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
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void btnCamera2_Clicked(object sender, EventArgs e)
        {
            var prospectID = entTempID.Text;

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
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void btnCamera3_Clicked(object sender, EventArgs e)
        {
            var prospectID = entTempID.Text;

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
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private async void btnCamera4_Clicked(object sender, EventArgs e)
        {
            var prospectID = entTempID.Text;
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
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
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

                        acPage4.IsVisible = true;
                        acPage3.IsVisible = false;
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

        private void SyncStatus(string status)
        {
            Device.BeginInvokeOnMainThread(() => {
                sendStatus.Text = status;
            });
        }
    }
}