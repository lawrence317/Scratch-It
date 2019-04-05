using Microsoft.AppCenter.Crashes;
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
                    Crashes.TrackError(ex);
                    await DisplayAlert("App Error", ex.Message.ToString(), "ok");
                }
            }
        }

        public void GetCafDetails(string caf)
        {
            try
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
                    lblActivityLocation.Text = contactResult.GPSCoordinates;
                    lblLastUpdated.Text = contactResult.LastUpdated.ToString("MM/dd/yyyy HH:mm:ss");
                    lblLastSync.Text = contactResult.LastSync.ToString("MM/dd/yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public void GetRekorida(string caf)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getCaf = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00001'", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    lblRekorida.Text = "True";
                }
                else
                {
                    lblRekorida.Text = "False";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public void GetMerchandizing(string caf)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getCaf = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00002'", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    lblMerchandizing.Text = "True";
                }
                else
                {
                    lblMerchandizing.Text = "False";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public void GetTradeCheck(string caf)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getCaf = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00003'", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    lblTradeCheck.Text = "True";
                }
                else
                {
                    lblTradeCheck.Text = "False";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public void GetOthers(string caf)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getCaf = conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00004'", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    lblOthers.Text = "True";
                }
                else
                {
                    lblOthers.Text = "False";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public void GetLocation(string code)
        {
            try
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public void GetRetailerName(string code)
        {
            try
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

                    if (nameResultCount > 0)
                    {
                        var nameResult = getName.Result[0];
                        lblRetailerName.Text = nameResult.FileAs;
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }
    }
}