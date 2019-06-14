using Plugin.DeviceInfo;
using System.Net.Http;
using Xamarin.Essentials;

namespace TBSMobile.Data
{
    public class Constants
    {
        public static string hostname = "LOCALHOST";
        public static string database = "LIVE";
        public static string apifolder = "TBSApp";

        public static string deviceID = CrossDeviceInfo.Current.Id;
        public static string appversion = "App Version: " + VersionTracking.CurrentVersion;
        public static string email = "lawrenceagulto.317@gmail.com";
        public static HttpClient client = new HttpClient();

        public class connections
        {
            HttpClient client = new HttpClient();
        }
    }
}
