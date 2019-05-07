//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

namespace Canti.Blockchain
{
    // DEBUG / INCOMPLETE
    public partial class BlockchainStorage
    {
        // Returns a block based on height
        Block GetBlock(int Height) { return default(Block); }

        // Returns a block based on hash
        Block GetBlock(string BlockHash) { return default(Block); }

        /*
        Transaction GetTransaction(string TransactionHash) { return default(Transaction); }
        Transaction[] GetTransactionWithPaymentId(string PaymentId) { return null; }
        Transaction[] GetTransactionInBlock(string BlockHash) { return null; }
        Input[] GetInputs() { return null; }
        Output GetOutput() { return default(Output); }
        */
    }
}
