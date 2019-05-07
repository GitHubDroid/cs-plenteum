//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Database;
using System.Collections.Generic;

namespace Canti.Blockchain
{
    // DEBUG / INCOMPLETE
    // Handles blockchain storage
    public partial class BlockchainStorage
    {
        // Database to store with
        private IDatabase Database;

        // SQL command strings
        private string INSERT_BLOCK;

        // Create an BlockStorage instance using a specified database handler
        public BlockchainStorage(IDatabase Database)
        {
            // Set database handler
            this.Database = Database;

            // Create tables if not existing in the database (blocks, transactions, transaction pool,
            // inputs, outputs, outputs index maximums, information

            // TODO - DB Storage transactions (promises) for multiple access points
        }
    }
}
