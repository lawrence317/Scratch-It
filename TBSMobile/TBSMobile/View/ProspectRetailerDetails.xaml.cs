using Microsoft.AppCenter.Crashes;
using System;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProspectRetailerDetails : ContentPage
    {
        public ProspectRetailerDetails(ContactsTable item)
        {
            InitializeComponent();
            GetContactDetails(item.ContactID);
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

        public void GetContactDetails(string contact)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getContact = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactID=?", contact);
                var contactResultCount = getContact.Result.Count;

                if (contactResultCount > 0)
                {
                    var contactResult = getContact.Result[0];

                    var getProvince = conn.QueryAsync<ProvinceTable>("SELECT * FROM tblProvince WHERE ProvinceID=?", contactResult.PresProvince);
                    var getProvinceResultCount = getProvince.Result.Count;

                    var province = "";

                    if (getProvinceResultCount > 0)
                    {
                        var provinceResult = getProvince.Result[0];
                        province = provinceResult.Province;
                    }
                    else
                    {
                        province = "N/A";
                    }

                    var town = "";

                    var getTown = conn.QueryAsync<TownTable>("SELECT * FROM tblTown WHERE TownID=?", contactResult.PresTown);
                    var getTownResultCount = getTown.Result.Count;
                    if (getTownResultCount > 0)
                    {
                        var townResult = getTown.Result[0];
                        town = townResult.Town;
                    }
                    else
                    {
                        town = "N/A";
                    }

                    lblFullName.Text = contactResult.FileAs;
                    lblAddress.Text = contactResult.PresBarangay + ", " + contactResult.PresStreet + ", " + town + ", " + province + ", " + contactResult.PresCountry + " - " + contactResult.PresDistrict;
                    lblLandmark.Text = contactResult.Landmark;
                    lblTelephone1.Text = contactResult.Telephone1;
                    lblTelephone2.Text = contactResult.Telephone2;
                    lblMobile.Text = contactResult.Mobile;
                    lblEmail.Text = contactResult.Email;
                    lblRemarks.Text = contactResult.CustomerRemarks;
                    lblLastUpdated.Text = contactResult.LastUpdated.ToString("MM/dd/yyyy HH:mm:ss");
                    lblLastSync.Text = contactResult.LastSync.ToString("MM/dd/yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}