using System;
using System.Collections.Generic;
using System.Text;
using iBoxDB.LocalServer;

namespace Plenteum.Database
{
    public abstract class BaseRepository
    {
        internal DB _db;
        public BaseRepository(DB db)
        {
            _db = db;
        }
    }
}
