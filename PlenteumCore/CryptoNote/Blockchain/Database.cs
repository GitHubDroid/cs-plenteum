//
// Copyright (c) 2018-2019 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

namespace Plenteum.CryptoNote
{
    internal sealed partial class BlockchainCache
    {
        // The database we will use to store information
        private IDatabase Database { get; set; }

        // Starts the database and sets up associated tables
        private void StartDatabase(IDatabase Database)
        {
            // Assign and start database
            this.Database = Database;

            Logger.Debug("Setting up the database...");
            this.Database.Start();

            Logger.Debug("Database setup completed");

        }

        // Stops the database
        private void StopDatabase()
        {
            Database.Stop();
        }
    }
}
