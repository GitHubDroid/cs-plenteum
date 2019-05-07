//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Canti.Database
{
    public static class Utilities
    {
        // Returns a SQL data type based on system object type
        public static string GetSqlType(object Value)
        {
            // Create a sqlite parameter to try to parse type
            SqliteParameter p = new SqliteParameter("", Value);
            string Type = "";
            try { Type = p.SqliteType.ToString(); }
            catch { Type = SqlDbType.Image.ToString(); }
            return Type;
        }
    }
}
