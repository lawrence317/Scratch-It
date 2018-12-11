
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android;
using Android.Views;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace TBSMobile.Droid
{
    [Activity(Label = "Scratch It!", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;

            base.OnCreate(bundle);

            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            AppCenter.Start("8fb4b039-f3b8-4910-9cb2-041eb9b80177", typeof(Analytics), typeof(Crashes), typeof(Distribute));


            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
                {
                    RequestPermissions(new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.WakeLock, Manifest.Permission.AccessFineLocation, Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage, Manifest.Permission.LocationHardware, Manifest.Permission.AccessMockLocation, Manifest.Permission.CaptureVideoOutput, Manifest.Permission.CaptureSecureVideoOutput, Manifest.Permission.AccessNetworkState, Manifest.Permission.AccessWifiState}, 0);
                }
            }
        }
    }
}

