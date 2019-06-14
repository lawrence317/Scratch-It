using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.Geolocator;
using System;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        string contact;
        string host;
        string database;
        string ipaddress;

        public MainMenu(string host, string database, string contact, string ipaddress)
        {
            InitializeComponent();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;

            CheckConnectionContinuously();
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

                        await App.TodoManager.CheckContactsData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckRetailerOutletData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckCAFData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckCAFActivityData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckEmailRecipientData(host, database, ipaddress, contact);

                        var isfirsttimesync = Preferences.Get("isfirsttimesync", String.Empty, "private_prefs");
                        if (isfirsttimesync == "1")
                        {
                            Online_Text();
                            Disable_UI();

                            await DisplayAlert("Application Error", "Please complete the syncing process before you can continue", "Ok");

                            await Navigation.PopToRootAsync();
                        }
                        else
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                Online_Text();
                                Disable_UI();

                                await App.TodoManager.CheckAutoSync(host, database, ipaddress, contact, SyncStatus);

                                Online_Text();
                                Enable_UI();
                            }
                            else
                            {
                                Offline_Text();
                                Enable_UI();
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

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void btnFAF_Clicked(object sender, EventArgs e)
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
                            Analytics.TrackEvent("Opened Field Activity Form");
                            await Navigation.PushAsync(new FieldActivityForm(host, database, contact, ipaddress));
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

        private async void btnPR_Clicked(object sender, EventArgs e)
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

                        Analytics.TrackEvent("Opened Add Prospect Retailer Form");
                        await Navigation.PushAsync(new ProspectRetailerList(host, database, contact, ipaddress));
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

        private async void btnRetailer_Clicked(object sender, EventArgs e)
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

                        Analytics.TrackEvent("Opened Add Retailer Outlet Form");
                        await Navigation.PushAsync(new RetailerList(host, database, contact, ipaddress));
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

        private async void btnLogout_Clicked(object sender, EventArgs e)
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
                        Analytics.TrackEvent("Logged Out");

                        var confirm = await DisplayAlert("Logout Confirmation", "Do you want to logout?", "Yes", "No");

                        if (confirm.Equals(true))
                        {
                            await Application.Current.MainPage.Navigation.PopToRootAsync();
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

        public async void CheckConnectionContinuously()
        {
            try
            {
                CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
                {
                    await App.TodoManager.CheckContactsData(host, database, ipaddress, contact);
                    await App.TodoManager.CheckRetailerOutletData(host, database, ipaddress, contact);
                    await App.TodoManager.CheckCAFData(host, database, ipaddress, contact);
                    await App.TodoManager.CheckCAFActivityData(host, database, ipaddress, contact);
                    await App.TodoManager.CheckEmailRecipientData(host, database, ipaddress, contact);

                    if (CrossConnectivity.Current.IsConnected)
                    {
                        Online_Text();
                        Disable_UI();

                        await App.TodoManager.CheckAutoSync(host, database, ipaddress, contact, SyncStatus);

                        Online_Text();
                        Enable_UI();
                    }
                    else
                    {
                        Offline_Text();
                        Enable_UI();
                    }
                };
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }

        }

        private async void btnUI_Clicked(object sender, EventArgs e)
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

                        await Navigation.PushAsync(new UnsyncedData(host, database, contact, ipaddress));
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

        private async void btnAH_Clicked(object sender, EventArgs e)
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

                        Analytics.TrackEvent("Opened Activity History");
                        await Navigation.PushAsync(new ActivityHistoryList(host, database, contact, ipaddress));
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

        private async void BtnResend_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string action = await DisplayActionSheet("What data do you want to re-sync?", "Cancel", null, "All Data", "Retailer Data Only", "Retailer Outlet Data Only", "CAF Data Only", "CAF Activity Data Only");

                    if (action == "All Data")
                    {
                        Disable_UI();

                        await App.TodoManager.ReSynContacts(host, database, ipaddress, contact, SyncStatus);
                        await App.TodoManager.ReSyncRetailerOutlet(host, database, ipaddress, contact, SyncStatus);
                        await App.TodoManager.ReSyncCAF(host, database, ipaddress, contact, SyncStatus);
                        await App.TodoManager.ReSyncCAFActivity(host, database, ipaddress, contact, SyncStatus);
                        await App.TodoManager.CheckContactsData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckRetailerOutletData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckCAFData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckCAFActivityData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckEmailRecipientData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckAutoSync(host, database, ipaddress, contact, SyncStatus);

                        Enable_UI();
                    }
                    else if (action == "Retailer Data Only")
                    {
                        Disable_UI();

                        await App.TodoManager.ReSynContacts(host, database, ipaddress, contact, SyncStatus);
                        await App.TodoManager.CheckContactsData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckAutoSync(host, database, ipaddress, contact, SyncStatus);

                        Enable_UI();
                    }
                    else if (action == "Retailer Outlet Data Only")
                    {
                        Disable_UI();

                        await App.TodoManager.ReSyncRetailerOutlet(host, database, ipaddress, contact, SyncStatus);
                        await App.TodoManager.CheckRetailerOutletData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckAutoSync(host, database, ipaddress, contact, SyncStatus);

                        Enable_UI();
                    }
                    else if (action == "CAF Data Only")
                    {
                        Disable_UI();

                        await App.TodoManager.ReSyncCAF(host, database, ipaddress, contact, SyncStatus);
                        await App.TodoManager.CheckCAFData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckAutoSync(host, database, ipaddress, contact, SyncStatus);

                        Enable_UI();
                    }
                    else if (action == "CAF Activity Data Only")
                    {
                        Disable_UI();

                        await App.TodoManager.ReSyncCAFActivity(host, database, ipaddress, contact, SyncStatus);
                        await App.TodoManager.CheckCAFActivityData(host, database, ipaddress, contact);
                        await App.TodoManager.CheckAutoSync(host, database, ipaddress, contact, SyncStatus);

                        Enable_UI();
                    }
                }
                else
                {
                    await DisplayAlert("Re-sync Data Error", "Re-sync failed. Please connect to the internet to re-sync your data", "Ok");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        private void SyncStatus(string status)
        {
            Device.BeginInvokeOnMainThread(() => {
                lblStatus.Text = status;
            });
        }

        public void Disable_UI()
        {
            btnFAF.IsEnabled = false;
            btnAH.IsEnabled = false;
            btnLogout.IsEnabled = false;
            btnUI.IsEnabled = false;
            btnPR.IsEnabled = false;
            btnRetailer.IsEnabled = false;
            btnResend.IsEnabled = false;
        }

        public void Enable_UI()
        {
            btnFAF.IsEnabled = true;
            btnAH.IsEnabled = true;
            btnLogout.IsEnabled = true;
            btnUI.IsEnabled = true;
            btnPR.IsEnabled = true;
            btnRetailer.IsEnabled = true;
            btnResend.IsEnabled = true;
        }

        public void Offline_Text()
        {
            lblStatus.Text = "Offline - Connect to internet";
            lblStatus.BackgroundColor = Color.FromHex("#e74c3c");
        }

        public void Online_Text()
        {
            lblStatus.Text = "Online - Connected to server";
            lblStatus.BackgroundColor = Color.FromHex("#2ecc71");
        }
    }
}