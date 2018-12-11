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
                // Look at releaseDetails public properties to get version information, release notes text or release notes URL
                string versionName = releaseDetails.ShortVersion;
                string versionCodeOrBuildNumber = releaseDetails.Version;
                string releaseNotes = releaseDetails.ReleaseNotes;
                Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

                // custom dialog
                var title = "Version " + versionName + " available!";
                Task answer;

                // On mandatory update, user cannot postpone
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
                    // If mandatory or if answer was positive
                    if (releaseDetails.MandatoryUpdate || (task as Task<bool>).Result)
                    {
                        // Notify SDK that user selected update
                        Distribute.NotifyUpdateAction(UpdateAction.Update);
                    }
                    else
                    {
                        // Notify SDK that user selected postpone (for 1 day)
                        // Note that this method call is ignored by the SDK if the update is mandatory
                        Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                    }
                });

                // Return true if you are using your own dialog, false otherwise
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
