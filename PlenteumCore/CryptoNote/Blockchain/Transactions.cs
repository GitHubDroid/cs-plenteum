//
// Copyright (c) 2018-2019 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Plenteum.CryptoNote.Blockchain;
using static Plenteum.Utils;

namespace Plenteum.CryptoNote
{
    internal sealed partial class BlockchainCache
    {
        #region Methods

        #region Database

        // Helper for easier deserialization, so this doesn't need re-typed
        private Transaction DeserializeTransactionRow(ValueList Row)
        {
            return new Transaction()
            {
                Version = (byte)Row["version"],
                Hash = (string)Row["hash"],
                BlockHash = (string)Row["block_hash"],
                PublicKey = (string)Row["public_key"],
                Size = (ulong)Row["size"],
                TotalAmount = (ulong)Row["total_amount"],
                TotalFee = (ulong)Row["fee"],
                Mixin = (byte)Row["mixin"],
                UnlockTime = (ulong)Row["unlock_time"],
                Verified = (int)Row["verified"] == 1,

                // TODO - deserialize input and output arrays from bytes
                Inputs = ByteArrayToObject<Input[]>((byte[])Row["inputs"]),
                Outputs = ByteArrayToObject<Output[]>((byte[])Row["outputs"]),
                Signatures = ByteArrayToObject<byte[]>((byte[])Row["signatures"]),
                Extra = (byte[])Row["extra"]
            };
        }

        // Gets a transaction from the database by hash
        internal bool TryGetTransaction(string Hash, out Transaction Transaction)
        {
            // Check if database is started
            if (!Database.Started)
            {
                Transaction = new Transaction()
                {
                    Hash = Constants.NULL_HASH
                };
                return false;
            }

            // Query the database
            var Result = Database.TransactionRepository.Find(new ValueList
            {
                ["Hash"] = Hash
            });

            // If there are no matching entries, return false along with an empty transaction
            if (Result == null)
            {
                Transaction = new Transaction()
                {
                    Hash = Constants.NULL_HASH
                };
                return false;
            }
            //TODO: Get Inputs and Outputs for this Transaction

            // Serialize found transaction from first row of result
            Transaction = Result;
            return true;
        }

        internal bool TryGetTransactionsByBlockHash(string Hash, out List<Transaction> Transactions)
        {
            // Check if database is started
            Transactions = new List<Transaction>();
            if (!Database.Started)
            {
                return false;
            }

            // Query the database
            var Result = Database.TransactionRepository.FindAll(new ValueList
            {
                ["BlockHash"] = Hash
            });

            // If there are no matching entries, return false along with an empty transaction
            if (Result == null)
            {
                return false;
            }
            //TODO: Get Inputs and Outputs for this Transaction


            // Serialize found transaction from first row of result
            Transactions.AddRange(Result);
            return true;
        }

        // Stores a transaction in the database
        internal bool StoreTransaction(Block Block, Transaction Transaction)
        {
            // Check if database is started
            if (!Database.Started) return false;

            // Store transaction
            Database.TransactionRepository.Add(Transaction);
            //TODO:
            //Store Inputs

            //Store Outputs
            // Completed
            return true;
        }

        // Checks if we have a transaction with this hash stored
        internal bool IsTransactionStored(string Hash)
        {
            // Check if database is started
            if (!Database.Started) return false;

            // Count the instances of this transaction hash
            var Count = Database.TransactionRepository.Count(new ValueList
            {
                ["hash"] = Hash
            });
            return Count > 0;
        }

        #endregion

        #endregion
    }
}
