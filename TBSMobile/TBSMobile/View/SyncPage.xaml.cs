using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncPage : ContentPage
    {
        string contact;
        string host;
        string database;
        string ipaddress;

        public SyncPage(string host, string database, string contact, string ipaddress)
        {
            InitializeComponent();
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var isfirsttimesync = Preferences.Get("isfirsttimesync", String.Empty, "private_prefs");

            if (CrossConnectivity.Current.IsConnected)
            {
                if (isfirsttimesync == "1")
                {
                    await App.TodoManager.FirstTimeSyncUser(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.FirstTimeSyncSystemSerial(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.FirstTimeSyncContacts(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.FirstTimeSyncRetailerOutlet(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.FirstTimeSyncCAF(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.FirstTimeSyncCAFActivity(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.FirstTimeSyncEmailRecipient(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.FirstTimeSyncProvince(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.FirstTimeSyncTown(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncUserLogsClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.OnSyncComplete(host, database, ipaddress, contact);
                    
                }
                else
                {
                    await App.TodoManager.SyncUserClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.UpdateContacts(contact);
                    await App.TodoManager.SyncContactsClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncContactsMedia1ClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncContactsMedia2ClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncContactsMedia3ClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncContactsMedia4ClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncRetailerOutletClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.UpdateCAF(contact);
                    await App.TodoManager.SyncCAFClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncCAFMedia1ClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncCAFMedia2ClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncCAFMedia3ClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncCAFMedia4ClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncCAFActivityClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncEmailRecipientClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncUserServerUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncSystemSerialServerUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncContactsServerUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncRetailerOutletServerUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncProvinceServerUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncTownServerUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.SyncUserLogsClientUpdate(host, database, ipaddress, contact, SyncStatus);
                    await App.TodoManager.OnSyncComplete(host, database, ipaddress, contact);
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
                    await Application.Current.MainPage.Navigation.PushAsync(new MainMenu(host, database, contact, ipaddress));
                }
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
            await Application.Current.MainPage.Navigation.PushAsync(new MainMenu(host, database, contact, ipaddress));
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("username", String.Empty, "private_prefs");
            Preferences.Set("password", String.Empty, "private_prefs");
            await Navigation.PopToRootAsync();
        }
    }
}