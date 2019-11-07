//
// Copyright (c) 2018-2019 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System.Collections.Generic;
using Plenteum.Cryptography;

namespace Plenteum.CryptoNote.Blockchain.Models
{
    // Block template for local storage
    public class Block
    {
        #region Properties and Fields
        public string Hash { get; set; }

        #region Header

        public uint MajorVersion { get; set; }
        public uint MinorVersion { get; set; }
        public ulong Timestamp { get; set; }
        public uint Nonce { get; set; }
        #endregion

        #region Convenience

        public uint Height { get; set; }
        public ulong BaseReward { get; set; }
        public ulong TotalFees { get; set; }
        public ulong AlreadyGeneratedUnits { get; set; }
        // Previous block info
        public virtual string PreviousBlockHash { get; set; }
        #endregion
        #endregion

        #region Constructors

        // Initializes an empty block
        public Block()
        {
            // Setup defaults
            Hash = Constants.NULL_HASH;
            PreviousBlockHash = Constants.NULL_HASH;
            Height = 0;
            Timestamp = 0;
            Nonce = 0;
            MajorVersion = Constants.BLOCK_MAJOR_VERSION_0;
            MinorVersion = Constants.BLOCK_MINOR_VERSION;
            BaseReward = 0;
            TotalFees = 0;
            AlreadyGeneratedUnits = 0;
            Timestamp = 0;
        }

        #endregion
    }
}
