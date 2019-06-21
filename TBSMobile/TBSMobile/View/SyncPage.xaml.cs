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
                        await App.TodoManager.FirstTimeSyncSystemSerial(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.FirstTimeSyncContacts(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.FirstTimeSyncRetailerOutlet(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.FirstTimeSyncCAF(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.FirstTimeSyncCAFActivity(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.FirstTimeSyncEmailRecipient(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.FirstTimeSyncProvince(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.FirstTimeSyncTown(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.OnSyncComplete(host, database, domain, contact);
                    }
                    else
                    {
                        await App.TodoManager.SyncUserClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.UpdateContacts(contact);
                        await App.TodoManager.SyncContactsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncContactsMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncContactsMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncContactsMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncContactsMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncRetailerOutletClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.UpdateCAF(contact);
                        await App.TodoManager.SyncCAFClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncCAFMedia1ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncCAFMedia2ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncCAFMedia3ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncCAFMedia4ClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncCAFActivityClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncEmailRecipientClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncUserServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncSystemSerialServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncContactsServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncRetailerOutletServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncProvinceServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncTownServerUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.SyncUserLogsClientUpdate(host, database, domain, apifolder, contact, SyncStatus);
                        await App.TodoManager.OnSyncComplete(host, database, domain, contact);
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
                syncStatus.Text = status;
            });
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void btnContinue_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu());
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("username", String.Empty, "private_prefs");
            Preferences.Set("password", String.Empty, "private_prefs");
            await Navigation.PopToRootAsync();
        }
    }
}