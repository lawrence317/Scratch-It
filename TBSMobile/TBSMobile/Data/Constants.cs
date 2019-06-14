using Plugin.DeviceInfo;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        public static readonly ISQLiteDB db = DependencyService.Get<ISQLiteDB>();
        public static readonly SQLite.SQLiteAsyncConnection conn = Constants.db.GetConnection();
    }
}
