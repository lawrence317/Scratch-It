﻿using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TBSMobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBSMobile.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActivityHistoryList : ContentPage
	{
        string contact;
        string host;
        string database;
        string ipaddress;
        

        public ActivityHistoryList (string host, string database, string contact, string ipaddress)
		{
			InitializeComponent ();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#f1c40f");
            this.contact = contact;
            this.host = host;
            this.database = database;
            this.ipaddress = ipaddress;
            
            GetActivity(contact);
        }

        public async void GetActivity(string contact)
        {
            try
            {
                var db = DependencyService.Get<ISQLiteDB>();
                var conn = db.GetConnection();

                var getActivity = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE EmployeeID = ? AND LastSync >= LastUpdated ORDER BY CAFDate DESC, StartTime DESC", contact);
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
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
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
                        Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
                    }
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
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

                var getActivity = conn.QueryAsync<CAFTable>("SELECT * FROM tblCaf WHERE EmployeeID=? AND LastSync >= LastUpdated ORDER BY CAFDate DESC, StartTime DESC LIMIT 100", contact);
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
                Crashes.TrackError(ex);  await DisplayAlert("Exception Error", ex.ToString(), "ok");
            }
        }
    }
}