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
	public partial class UnsyncedData : CarouselPage
    {
        string contact;
        string host;
        string database;
        string ipaddress;
        byte[] pingipaddress;

        public UnsyncedData (string host, string database, string contact, string ipaddress, byte[] pingipaddress)
		{
			InitializeComponent ();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#f1c40f");
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            this.pingipaddress = pingipaddress;
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
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }

        public void GetRetailerGroup(string contact)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getRetailer = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Coordinator=? AND LastUpdated > LastSync ORDER BY RetailerCode ASC", contact);
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

        public void GetActivity(string contact)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getActivity = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE EmployeeID=? AND LastUpdated > LastSync ORDER BY CAFNo ASC", contact);
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

        public void GetProspectRetailer(string contact)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Coordinator=? AND LastUpdated > LastSync ORDER BY FileAs ASC LIMIT 100", contact);
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

        private async void lstActivity_ItemTapped(object sender, ItemTappedEventArgs e)
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
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }

        private void lstActivity_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstActivity.SelectedItem = null;
        }

        private void lstActivity_Refreshing(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getActivity = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE EmployeeID=? AND LastUpdated > LastSync ORDER BY CAFNo ASC", contact);
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

        private async void lstProspect_ItemTapped(object sender, ItemTappedEventArgs e)
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
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }

        private void lstProspect_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstProspect.SelectedItem = null;
        }

        private void lstProspect_Refreshing(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getProspect = conn.QueryAsync<ContactsTable>("SELECT * FROM tblContacts WHERE ContactType = 'Prospect Retailer' AND Coordinator=? AND LastUpdated > LastSync ORDER BY FileAs ASC LIMIT 100", contact);
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

        private async void lstRetailerGroup_ItemTapped(object sender, ItemTappedEventArgs e)
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
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }

        private void lstRetailerGroup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstRetailerGroup.SelectedItem = null;
        }

        private void lstRetailerGroup_Refreshing(object sender, EventArgs e)
        {
            var db = DependencyService.Get<ISQLiteDB>();
            var conn = db.GetConnection();

            var getRetailer = conn.QueryAsync<RetailerGroupTable>("SELECT * FROM tblRetailerGroup WHERE Coordinator=? AND LastUpdated > LastSync ORDER BY RetailerCode ASC", contact);
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