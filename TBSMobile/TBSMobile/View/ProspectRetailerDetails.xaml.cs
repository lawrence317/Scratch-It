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
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }

        public void GetContactDetails(string contact)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getContact = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactID=?", contact);
            var contactResultCount = getContact.Result.Count;

            if (contactResultCount > 0)
            {
                var contactResult = getContact.Result[0];

                var getProvince = conn.QueryAsync<ProvinceTable>("SELECT * FROM tblProvince WHERE ProvinceID=?", contactResult.PresProvince);
                var provinceResult = getProvince.Result[0];

                var getTown = conn.QueryAsync<TownTable>("SELECT * FROM tblTown WHERE TownID=?", contactResult.PresTown);
                var townResult = getTown.Result[0];
                

                lblFullName.Text = contactResult.FileAs;
                lblAddress.Text = contactResult.PresBarangay + ", " + contactResult.PresStreet + ", " + townResult.Town + ", " + provinceResult.Province + ", " + contactResult.PresCountry + " - " + contactResult.PresDistrict;
                lblLandmark.Text = contactResult.Landmark;
                lblTelephone1.Text = contactResult.Telephone1;
                lblTelephone2.Text = contactResult.Telephone2;
                lblMobile.Text = contactResult.Mobile;
                lblEmail.Text = contactResult.Email;
            }
        }
    }
}