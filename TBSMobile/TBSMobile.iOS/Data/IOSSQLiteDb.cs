using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SQLite;
using TBSApp.iOS.Data;
using TBSMobile.Data;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSSQLiteDb))]

namespace TBSApp.iOS.Data
{
    public class IOSSQLiteDb : ISQLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var dbFileName = "backend.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, dbFileName);

            return new SQLiteAsyncConnection(path);
        }
    }
}