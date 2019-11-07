//
// Copyright (c) 2019, The Plenteum Developers
// 
// Please see the included LICENSE file for more information.

using iBoxDB.LocalServer;
using Plenteum.CryptoNote.Blockchain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using Plenteum.Database;

namespace Plenteum
{
    // TODO - finish summary labeling on anything public
    /// <summary>
    /// A SQLite database handler
    /// </summary>
    public sealed class IBoxDB : IDatabase
    {
        #region Properties and Fields

        #region Public

        public BlockRepository BlockRepository { get; set; }
        public TransactionRepository TransactionRepository { get; set; }

        /// <summary>
        /// This database's type
        /// </summary>
        public DatabaseType Type { get; private set; }

        /// <summary>
        /// Whether or not this database has been started
        /// </summary>
        public bool Started { get; private set; }

        #endregion

        #region Private

        // The file this database will read and write to/from
        private string DatabaseFilePath { get; set; }

        // The iBoxDB Database object
        private DB db { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Starts this database
        /// </summary>
        public void Start()
        {
            // Create sqlite connection
            db = new DB(DatabaseFilePath);

            var config = db.GetConfig();
            //initialise all the tables and indexes
            config.EnsureTable<Block>("Block", "Height");
            config.EnsureTable<Transaction>("Transaction", "Id");
            //config.EnsureTable<Input>("Input", "Id");
            //config.EnsureTable<Output>("Output", "Id");

            //initialise all the indexes
            //config.EnsureIndex<Block>("Block",  true, "Hash");
            config.EnsureIndex<Transaction>("Transaction", false, "PaymentId(64)");
            config.EnsureIndex<Transaction>("Transaction", false, "BlockHash(64)");
            //config.EnsureIndex<Input>("Input", "TxId");
            //config.EnsureIndex<Output>("Output", "TxId");

            //instatiate Repositories
            BlockRepository = new BlockRepository(db);
            TransactionRepository = new TransactionRepository(db);

            // Set as started
            Started = true;
        }

        /// <summary>
        /// Stops this database
        /// </summary>
        public void Stop()
        {
            // Set as stopped
            Started = false;

            //Dispose of iBoxDB connection
            db.Dispose();
        }
        


        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new IBoxDB database File
        /// </summary>
        public IBoxDB(string DatabaseFilePath)
        {
            // Set database type
            Type = DatabaseType.IBOXDB;

            // Set database file
            this.DatabaseFilePath = DatabaseFilePath;
        }

        #endregion
    }
}
