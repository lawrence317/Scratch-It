using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TBSMobile.Data
{
    public interface ISQLiteDB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
