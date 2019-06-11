using System;
using TBSMobile.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using System.Threading.Tasks;

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
            AppCenter.Start("android=423c8020-f1af-4fba-b62f-c07e1fb382b6;", typeof(Analytics), typeof(Crashes), typeof(Distribute));
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
