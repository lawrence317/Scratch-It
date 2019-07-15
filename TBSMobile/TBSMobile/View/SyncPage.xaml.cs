using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncPage : ContentPage
    {
        string host = Preferences.Get("host", String.Empty, "private_prefs");
        string database = Preferences.Get("database", String.Empty, "private_prefs");
        string domain = Preferences.Get("domain", String.Empty, "private_prefs");
        string apifolder = Preferences.Get("apifolder", String.Empty, "private_prefs");
        string contact = Preferences.Get("contactid", String.Empty, "private_prefs");

        public SyncPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var isfirsttimesync = Preferences.Get("isfirsttimesync", String.Empty, "private_prefs");

                if (CrossConnectivity.Current.IsConnected)
                {
                    if (isfirsttimesync == "1")
                    {
                        await App.TodoManager.FirstTimeSyncUser(host, database, domain, apifolder, contact, SyncStatus);
                    }
                    else
                    {
                        await App.TodoManager.SyncUserClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                    }
                }
                else
                {
                    if (isfirsttimesync == "1")
                    {
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
                    }
                }
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }

        }

        private void SyncStatus(string status)
        {
            Device.BeginInvokeOnMainThread(() => {

                try
                {
                    syncStatus.Text = status;
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            });
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void btnContinue_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                Preferences.Set("username", String.Empty, "private_prefs");
                Preferences.Set("password", String.Empty, "private_prefs");
                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}