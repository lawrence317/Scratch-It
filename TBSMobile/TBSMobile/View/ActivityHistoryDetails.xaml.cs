using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActivityHistoryDetails : ContentPage
	{
		public ActivityHistoryDetails (CAFTable item)
		{
			InitializeComponent ();
            GetCafDetails(item.CAFNo);
            GetRekorida(item.CAFNo);
            GetMerchandizing(item.CAFNo);
            GetTradeCheck(item.CAFNo);
            GetOthers(item.CAFNo);
            GetLocation(item.CustomerID);
            GetRetailerName(item.CustomerID);
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

        public void GetCafDetails(string caf)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getCaf = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE CAFNo=?", caf);
            var contactResultCount = getCaf.Result.Count;

            if (contactResultCount > 0)
            {
                var contactResult = getCaf.Result[0];
                lblCafNo.Text = contactResult.CAFNo;
                lblRetailerCode.Text = contactResult.CustomerID;
                lblDate.Text = contactResult.CAFDate.ToString("MM/dd/yyyy");
                lblStartTime.Text = contactResult.StartTime.ToString("hh:mm:ss");
                lblEndTime.Text = contactResult.EndTime.ToString("hh:mm:ss");
                lblOtherConcern.Text = contactResult.OtherConcern;
                lblRemarks.Text = contactResult.Remarks;
            }
        }

        public void GetRekorida(string caf)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getCaf = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND Activity = 'Rekorida'", caf);
            var contactResultCount = getCaf.Result.Count;

            if (contactResultCount > 0)
            {
                var contactResult = getCaf.Result[0];
                if(contactResult.ActivitySwitch == 1)
                {
                    lblRekorida.Text = "True";
                }
                else
                {
                    lblRekorida.Text = "False";
                }
            }
        }

        public void GetMerchandizing(string caf)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getCaf = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND Activity = 'Merchandizing'", caf);
            var contactResultCount = getCaf.Result.Count;

            if (contactResultCount > 0)
            {
                var contactResult = getCaf.Result[0];
                if (contactResult.ActivitySwitch == 1)
                {
                    lblMerchandizing.Text = "True";
                }
                else
                {
                    lblMerchandizing.Text = "False";
                }
            }
        }

        public void GetTradeCheck(string caf)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getCaf = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND Activity = 'Trade Check'", caf);
            var contactResultCount = getCaf.Result.Count;

            if (contactResultCount > 0)
            {
                var contactResult = getCaf.Result[0];
                if (contactResult.ActivitySwitch == 1)
                {
                    lblTradeCheck.Text = "True";
                }
                else
                {
                    lblTradeCheck.Text = "False";
                }
            }
        }

        public void GetOthers(string caf)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getCaf = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND Activity = 'Others'", caf);
            var contactResultCount = getCaf.Result.Count;

            if (contactResultCount > 0)
            {
                var contactResult = getCaf.Result[0];
                if (contactResult.ActivitySwitch == 1)
                {
                    lblOthers.Text = "True";
                }
                else
                {
                    lblOthers.Text = "False";
                }
            }
        }

        public void GetLocation(string code)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getCaf = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode=?", code);
            var contactResultCount = getCaf.Result.Count;

            if (contactResultCount > 0)
            {
                var contactResult = getCaf.Result[0];
                lblRetailerLocation.Text = contactResult.GPSCoordinates;
            }
        }

        public void GetRetailerName(string code)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getCaf = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode=?", code);
            var contactResultCount = getCaf.Result.Count;

            if (contactResultCount > 0)
            {
                var contactResult = getCaf.Result[0];
                var getName = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactID=?", contactResult.ContactID);
                var nameResultCount = getName.Result.Count;

                if(nameResultCount > 0)
                {
                    var nameResult = getName.Result[0];
                    lblRetailerName.Text = nameResult.FileAs;
                }
            }
        }
    }
}