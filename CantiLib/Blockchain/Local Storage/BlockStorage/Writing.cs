//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System.Collections.Generic;

namespace Canti.Blockchain
{
    // DEBUG / INCOMPLETE
    public partial class BlockchainStorage
    {
        // Stores a block
        void StoreBlock(Block Block)
        {
            // Create a dictionary of block values
            Dictionary<string, object> Variables = new Dictionary<string, object>();
            foreach (var Property in Block.GetType().GetProperties())
                Variables.Add(Property.Name, Property.GetValue(Block, null));

            // Execute command
            Database.ExecuteNonQuery(INSERT_BLOCK, Variables);
        }

        // Stores a group of blocks
        void StoreBlocks(Block[] Blocks)
        {
            // Iterate through blocks array and store each block
            foreach (var Block in Blocks) StoreBlock(Block);
        }

        /*
        void StoreTransaction(Transaction Transaction) { }
        void StoreTransactions(Transaction[] Transactions) { }
        void StoreInput(Input Input) { }
        void StoreInputs(Input[] Inputs) { }
        void StoreOutput(Output Output) { }
        void StoreOutputs(Output[] Outputs) { }
        */
    }
}
