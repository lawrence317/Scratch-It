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
            AppCenter.Start("android=8fb4b039-f3b8-4910-9cb2-041eb9b80177;", typeof(Analytics), typeof(Crashes), typeof(Distribute));
            Analytics.SetEnabledAsync(true);

            Distribute.ReleaseAvailable = OnReleaseAvailable;

            bool OnReleaseAvailable(ReleaseDetails releaseDetails)
            {
                string versionName = releaseDetails.ShortVersion;
                string versionCodeOrBuildNumber = releaseDetails.Version;
                string releaseNotes = releaseDetails.ReleaseNotes;
                Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;
                
                var title = "Version " + versionName + " available!";
                Task answer;
                
                if (releaseDetails.MandatoryUpdate)
                {
                    answer = Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install");
                }
                else
                {
                    answer = Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install", "Maybe tomorrow...");
                }
                answer.ContinueWith((task) =>
                {
                    if (releaseDetails.MandatoryUpdate || (task as Task<bool>).Result)
                    {
                        Distribute.NotifyUpdateAction(UpdateAction.Update);
                    }
                    else
                    {
                        Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                    }
                });
                
                return true;
            }
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
