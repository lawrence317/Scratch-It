using Microsoft.AppCenter.Crashes;
using System;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UnsyncedData : CarouselPage
    {
        string contact;
        string host;
        string database;
        string ipaddress;
        
        public UnsyncedData (string host, string database, string contact, string ipaddress)
		{
			InitializeComponent ();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#2ecc71");
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            
            GetRetailerGroup(contact);
            GetActivity(contact);
            GetProspectRetailer(contact);
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

        public async void GetRetailerGroup(string contact)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getRetailer = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Supervisor=? AND LastUpdated > LastSync ORDER BY RetailerCode ASC", contact);
                var resultCount = getRetailer.Result.Count;

                if (resultCount > 0)
                {
                    var result = getRetailer.Result;
                    lstRetailerGroup.ItemsSource = result;

                    lstRetailerGroup.IsVisible = true;
                    retailerIndicator.IsVisible = false;
                }
                else
                {
                    lstRetailerGroup.IsVisible = false;
                    retailerIndicator.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public async void GetActivity(string contact)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getActivity = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE EmployeeID=? AND LastUpdated > LastSync ORDER BY CAFDate DESC, StartTime DESC", contact);
                var resultCount = getActivity.Result.Count;

                if (resultCount > 0)
                {
                    var result = getActivity.Result;
                    lstActivity.ItemsSource = result;

                    lstActivity.IsVisible = true;
                    activityIndicator.IsVisible = false;
                }
                else
                {
                    lstActivity.IsVisible = false;
                    activityIndicator.IsVisible = true;
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        public async void GetProspectRetailer(string contact)
        {
            try
            {

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType = 'RT00004' AND Supervisor=? AND LastUpdated > LastSync ORDER BY FileAs ASC LIMIT 100", contact);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;

                    lstProspect.IsVisible = true;
                    prospectIndicator.IsVisible = false;
                }
                else
                {
                    lstProspect.IsVisible = false;
                    prospectIndicator.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private async void lstActivity_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
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

                            CAFTable item = (CAFTable)e.Item;

                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ActivityHistoryDetails(item))
                            {
                                BarBackgroundColor = Color.FromHex("#f1c40f")
                            });
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private void lstActivity_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstActivity.SelectedItem = null;
        }

        private async void lstActivity_Refreshing(object sender, EventArgs e)
        {
            try
            {

                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getActivity = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE EmployeeID=? AND LastUpdated > LastSync ORDER BY CAFDate, StartTime DESC LIMIT 50", contact);
                var resultCount = getActivity.Result.Count;

                if (resultCount > 0)
                {
                    var result = getActivity.Result;
                    lstActivity.ItemsSource = result;

                    lstActivity.IsVisible = true;
                    activityIndicator.IsVisible = false;
                }
                else
                {
                    lstActivity.IsVisible = false;
                    activityIndicator.IsVisible = true;
                }

                lstActivity.EndRefresh();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private async void lstProspect_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
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

                            ContactsTable item = (ContactsTable)e.Item;

                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProspectRetailerDetails(item))
                            {
                                BarBackgroundColor = Color.FromHex("#f1c40f")
                            });
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private void lstProspect_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstProspect.SelectedItem = null;
        }

        private async void lstProspect_Refreshing(object sender, EventArgs e)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE RetailerType = 'RT00004' AND Supervisor=? AND LastUpdated > LastSync ORDER BY FileAs ASC LIMIT 100", contact);
                var resultCount = getProspect.Result.Count;

                if (resultCount > 0)
                {
                    var result = getProspect.Result;
                    lstProspect.ItemsSource = result;

                    lstProspect.IsVisible = true;
                    prospectIndicator.IsVisible = false;
                }
                else
                {
                    lstProspect.IsVisible = false;
                    prospectIndicator.IsVisible = true;
                }

                lstProspect.EndRefresh();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private async void lstRetailerGroup_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
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

                            RetailerGroupTable item = (RetailerGroupTable)e.Item;

                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new RetailerGroupDetails(item))
                            {
                                BarBackgroundColor = Color.FromHex("#f1c40f")
                            });
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private void lstRetailerGroup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstRetailerGroup.SelectedItem = null;
        }

        private async void lstRetailerGroup_Refreshing(object sender, EventArgs e)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getRetailer = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Supervisor=? AND LastUpdated > LastSync ORDER BY RetailerCode ASC", contact);
                var resultCount = getRetailer.Result.Count;

                if (resultCount > 0)
                {
                    var result = getRetailer.Result;
                    lstRetailerGroup.ItemsSource = result;

                    lstRetailerGroup.IsVisible = true;
                    retailerIndicator.IsVisible = false;
                }
                else
                {
                    lstRetailerGroup.IsVisible = false;
                    retailerIndicator.IsVisible = true;
                }

                lstRetailerGroup.EndRefresh();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await DisplayAlert("App Error", ex.Message.ToString(), "ok");
            }
        }

        private void carouselPage_CurrentPageChanged(object sender, EventArgs e)
        {
            var pageCount = carouselPage.Children.Count;

            var index = carouselPage.Children.IndexOf(carouselPage.CurrentPage);
            if (index == 0)
            {
                Title = "Unsynced Field Activity";
            }
            else if(index == 1)
            {
                Title = "Unsynced Prospect Retailer";
            }
            else if (index == 2)
            {
                Title = "Unsynced Retailer Outlet";
            }
        }
    }
}