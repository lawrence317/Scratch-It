using Microsoft.AppCenter.Crashes;
using System;
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
            GetRapport(item.CAFNo);
            GetStock(item.CAFNo);
            GetReplenish(item.CAFNo);
            GetRetouch(item.CAFNo);
            GetFeed(item.CAFNo);
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

        public async void GetCafDetails(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE CAFNo=?", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    var contactResult = getCaf.Result[0];
                    lblCafNo.Text = contactResult.CAFNo;
                    lblRetailerCode.Text = contactResult.CustomerID;
                    lblDate.Text = contactResult.CAFDate.ToString("MM/dd/yyyy");
                    lblStartTime.Text = contactResult.StartTime.ToString("hh:mm:ss");
                    lblEndTime.Text = contactResult.EndTime.ToString("hh:mm:ss");

                    if (String.IsNullOrEmpty(contactResult.Remarks))
                    {
                        lblRemarks.Text = "none";
                    }
                    else
                    {
                        lblRemarks.Text = contactResult.Remarks;
                    }

                    if (String.IsNullOrEmpty(contactResult.OtherConcern))
                    {
                        lblOtherConcern.Text = "none";
                    }
                    else
                    {
                        lblOtherConcern.Text = contactResult.OtherConcern;
                    }

                    if (String.IsNullOrEmpty(contactResult.Feedback))
                    {
                        lblFeedback.Text = "none";
                    }
                    else
                    {
                        lblFeedback.Text = contactResult.Feedback;
                    }

                    lblActivityLocation.Text = contactResult.GPSCoordinates;
                    lblLastUpdated.Text = contactResult.LastUpdated.ToString("MM/dd/yyyy HH:mm:ss");
                    lblLastSync.Text = contactResult.LastSync.ToString("MM/dd/yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetRekorida(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00001'", caf);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetMerchandizing(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00002'", caf);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetTradeCheck(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00003'", caf);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetOthers(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00004'", caf);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetRapport(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00005'", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    lblRaport.Text = "True";
                }
                else
                {
                    lblRaport.Text = "False";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetStock(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00006'", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    lblStock.Text = "True";
                }
                else
                {
                    lblStock.Text = "False";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetReplenish(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00007'", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    lblReplenish.Text = "True";
                }
                else
                {
                    lblReplenish.Text = "False";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetRetouch(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00008'", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    lblRetouch.Text = "True";
                }
                else
                {
                    lblRetouch.Text = "False";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetFeed(string caf)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<ActivityTable>("SELECT * FROM tblActivity WHERE CAFNo=? AND ActivityID = 'ACT00009'", caf);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    lblFeed.Text = "True";
                }
                else
                {
                    lblFeed.Text = "False";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetLocation(string code)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode=?", code);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    var contactResult = getCaf.Result[0];
                    lblRetailerLocation.Text = contactResult.GPSCoordinates;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }

        public async void GetRetailerName(string code)
        {
            try
            {
                var getCaf = Constants.conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE RetailerCode=?", code);
                var contactResultCount = getCaf.Result.Count;

                if (contactResultCount > 0)
                {
                    var contactResult = getCaf.Result[0];
                    var getName = Constants.conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactID=?", contactResult.ContactID);
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
                Crashes.TrackError(ex);
                await DisplayAlert("Application Error", "Error:\n\n" + ex.Message.ToString() + "\n\n Please contact your administrator", "Ok");
            }
        }
    }
}