using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace TBSMobile.Data
{
    public class Constants
    {
        public static string hostname = "LOCALHOST";
        public static string database = "LIVE";
        public static string port = "7777";
        public static string apifolder = "TBSApp";

        public static string server_ip = "192.168.0.8";

        public static string deviceID = CrossDeviceInfo.Current.Id;
        public static string appversion = "App Version: " + VersionTracking.CurrentVersion;
        public static string email = "lawrenceagulto.317@gmail.com";

        public static string requestUrl = ":7777/TBSApp/mobile-api-new.php?";
    }
}
