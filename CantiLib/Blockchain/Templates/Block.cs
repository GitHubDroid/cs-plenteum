//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System;

namespace Canti.Blockchain
{
    // Block template
    [Serializable]
    public struct Block
    {
        // Block header
        [Serializable]
        public struct BlockHeader
        {
            public byte MajorVersion { get; set; }
            public byte MinorVersion { get; set; }
            public uint Nonce { get; set; }
            public ulong Timestamp { get; set; }
            public string PreviousBlockHash { get; set; }
        }
        public BlockHeader Header { get; set; }

        // Block data
        public bool IsAlternative { get; set; }
        public uint Height { get; set; }
        public string Hash { get; set; }
        public ulong Difficulty { get; set; }
        public ulong Reward { get; set; }
        public ulong BaseReward { get; set; }
        public ulong BlockSize { get; set; }
        public ulong TransactionCumulativeSize { get; set; }
        public ulong AlreadyGeneratedCoins { get; set; }
        public ulong AlreadyGeneratedTransactions { get; set; }
        public ulong SizeMedian { get; set; }
        public double Penalty { get; set; }
        public ulong TotalFeeAmount { get; set; }
        public Transaction[] Transactions { get; set; }

        public string[] TransactionHashes { get; set; }

        public Transaction BaseTransaction { get; set; }
        public uint TransactionCount { get; set; }
        public string[] BaseTransactionBranch { get; set; }
        public string[] BlockChainBranch { get; set; }
    }
}
