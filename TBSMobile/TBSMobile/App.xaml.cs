using System;
using TBSMobile.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace TBSMobile
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

		protected override void OnStart ()
		{
            AppCenter.Start("android=8fb4b039-f3b8-4910-9cb2-041eb9b80177;" + "uwp={Your UWP App secret here};" + "ios={Your iOS App secret here}", typeof(Analytics), typeof(Crashes), typeof(Distribute));
            Analytics.SetEnabledAsync(true);
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
