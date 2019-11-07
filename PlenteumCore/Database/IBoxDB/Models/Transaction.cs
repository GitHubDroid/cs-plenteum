//
// Copyright (c) 2018-2019 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System.Collections.Generic;
using Plenteum.Cryptography;
using Plenteum.CryptoNote.Blockchain;
using static Plenteum.Utils;

namespace Plenteum.CryptoNote.Blockchain.Models
{
    public class Transaction
    {
        #region Properties and Fields
        public string Hash { get; set; }

        // Hashed Values
        public byte Version { get; set; }
        public ulong UnlockTime { get; set; }
        public string PublicKey { get; set; }
        public byte[] Extra { get; set; }
        // Convenience
        public string BlockHash { get; set; }
        public string PaymentId { get; set; }
        public ulong Size { get; set; }
        public ulong TotalAmount { get; set; }
        public ulong TotalFee { get; set; }
        public byte Mixin { get; set; }

        // Verification
        public byte[] Signatures { get; set; }

        #endregion

        #region Constructors

        // Initializes an empty transaction
        public Transaction()
        {
            Version = Constants.TRANSACTION_VERSION;
            Hash = Constants.NULL_HASH;
            PaymentId = Constants.NULL_HASH;
            Size = 0;
            TotalAmount = 0;
            TotalFee = 0;
            Mixin = 0;
            UnlockTime = 0;
            Signatures = new byte[0];
            Extra = new byte[0];
        }

        #endregion
    }
}
