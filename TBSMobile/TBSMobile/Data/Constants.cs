using Plugin.DeviceInfo;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TBSMobile.Data
{
    public class Constants
    {
        public static string hostname = "localhost";
        public static string livedatabase = "live";
        public static string testdatabase = "test";
        public static string liveapifolder = "app_api";
        public static string testapifolder = "test_api";
        public static string domain = "tbs.scratchit.ph";

        public static string deviceID = CrossDeviceInfo.Current.Id;
        public static string appversion = "Version: " + VersionTracking.CurrentVersion;
        public static string email = "lawrenceagulto.317@gmail.com";

        public static readonly ISQLiteDB db = DependencyService.Get<ISQLiteDB>();
        public static readonly SQLite.SQLiteAsyncConnection conn = db.GetConnection();
    }
}
