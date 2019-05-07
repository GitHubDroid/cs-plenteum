//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System;
using System.Collections.Generic;

namespace Canti.Database
{
    // Interface for handling database connections and storage
    public interface IDatabase : IDisposable
    {
        // Querying
        void ExecuteNonQuery(string Command, Dictionary<string, object> Variables);
        List<Dictionary<string, object>> ExecuteQuery(string Command, Dictionary<string, object> Variables);
    }
}
