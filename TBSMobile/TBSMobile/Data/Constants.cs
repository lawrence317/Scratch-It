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
        public static string database = "DEMO";

        //public static string server_ip = "192.168.120.10";
        //public static string server_ip = "192.168.1.17";
        public static string server_ip = "192.168.0.8";

        public static string deviceID = CrossDeviceInfo.Current.Id;
        public static string appversion = "Version: " + VersionTracking.CurrentVersion;
        public static string email = "lawrenceagulto.317@gmail.com";

        public static string requestUrl = ":7777/TBSApp/mobile-api.php?";
    }
}
