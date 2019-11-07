//
// Copyright (c) 2018-2019 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System.Collections.Generic;
using System.Linq;
using Plenteum.CryptoNote.Blockchain;
using static Plenteum.Utils;

namespace Plenteum.CryptoNote
{
    // Handles all blockchain and storage operations
    internal sealed partial class BlockchainCache
    {
        #region Properties and Fields

        #region Internal

        // The current height of the blockchain
        internal uint Height
        {
            get
            {
                // Check if database is started
                if (!Database.Started)
                {
                    return 0;
                }

                // Count how many blocks are in the table
                // TODO - this can be made more efficient by only wuerying for the last block's height
                return (uint)Database.BlockRepository.Count();
            }
        }

        // The last known height of the blockchain
        private uint _knownHeight = 0;
        internal uint KnownHeight
        {
            get
            {
                // Check if last known height has not been assigned
                if (_knownHeight < 1) return Height;

                // Otherwise return assigned value
                else return _knownHeight;
            }
            set
            {
                // Cache assigned value
                _knownHeight = value;
            }
        }

        // The hash of the last stored block
        internal string LastBlockHash
        {
            get
            {
                // Try to get the block at the current height
                if (!TryGetBlock(Height, out Block Block)) return Constants.NULL_HASH;

                // Return last block hash
                return Block.Hash;
            }
        }

        // A byte array representation of the last stored block's hash
        internal byte[] TopBlockHash
        {
            get
            {
                return HexStringToByteArray(LastBlockHash);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Database


        // Helper for easier deserialization, so this doesn't need re-typed
        private Block GetBlockTransactions(Block block)
        {
            if (block.BaseTransaction != null && block.BaseTransaction.Hash != Constants.NULL_HASH)
            {
                TryGetTransaction(block.BaseTransaction.Hash, out Transaction BaseTransaction);
                block.SetBaseTransaction(BaseTransaction);
                //TODO: get other transactions

            }
            if (block.BaseTransaction == null || block.BaseTransaction.Hash == Constants.NULL_HASH)
            {
                TryGetTransactionsByBlockHash(block.Hash, out List<Transaction> Transactions);
                block.SetTransactions(Transactions);
            }
            

            

            return block;
        }

        // Gets a block hash by height
        internal string GetBlockHash(uint Height)
        {
            // Check if database is started
            if (!Database.Started)
            {
                return Constants.NULL_HASH;
            }

            // Query the database
            var Result = Database.BlockRepository.Find(new ValueList
            {
                ["Height"] = Height
            });

            // If there are no matching entries, return a null hash
            if (Result == null)
            {
                return Constants.NULL_HASH;
            }

            // Return the found block hash
            return Result.Hash;
        }

        // Gets a block from the database by height
        internal bool TryGetTopBlock(out Block Block)
        {
            // Check if database is started
            if (!Database.Started)
            {
                Block = new Block(this)
                {
                    Hash = Constants.NULL_HASH
                };
                return false;
            }

            // Query the database
            var Result = Database.BlockRepository.FindTopBlock();

            // If there are no matching entries, return false along with an empty block
            if (Result == null)
            {
                Block = new Block(this)
                {
                    Hash = Constants.NULL_HASH
                };
                return false;
            }
            //TODO: add Block Transactions.... 

            // return block from first row of result
            Block = Result;
            return true;
        }

        internal bool TryGetBlock(uint Height, out Block Block)
        {
            // Check if database is started
            if (!Database.Started)
            {
                Block = new Block(this)
                {
                    Hash = Constants.NULL_HASH
                };
                return false;
            }

            // Query the database
            var Result = Database.BlockRepository.Find(new ValueList
            {
                ["Height"] = Height
            });

            // If there are no matching entries, return false along with an empty block
            if (Result == null)
            {
                Block = new Block(this)
                {
                    Hash = Constants.NULL_HASH
                };
                return false;
            }

            // Serialize found block from first row of result
            Block = GetBlockTransactions(Result);
            return true;
        }

        // Gets a block from the database by hash
        internal bool TryGetBlock(string Hash, out Block Block)
        {
            // Check if database is started
            if (!Database.Started)
            {
                Block = new Block(this)
                {
                    Hash = Constants.NULL_HASH
                };
                return false;
            }

            // Query the database
            var Result = Database.BlockRepository.Find(new ValueList
            {
                ["hash"] = Hash
            });

            // If there are no matching entries, return false along with an empty block
            if (Result == null)
            {
                Block = new Block(this)
                {
                    Hash = Constants.NULL_HASH
                };
                return false;
            }

            // Serialize found block from first row of result
            Block = GetBlockTransactions(Result);
            return true;
        }

        // Stores a block in the database at the specified height
        internal bool StoreBlock(Block Block)
        {
            // Check if database is started
            if (!Database.Started) return false;

            // Calculate already generated units
            // TODO - use reward not base reward
            ulong AlreadyGeneratedUnits = GetAlreadyGeneratedUnits() + Block.BaseReward;

            // Store block
            Database.BlockRepository.Add(Block);

            // Store transactions in this block
            StoreTransaction(Block, Block.BaseTransaction);
            foreach (Transaction Transaction in Block.Transactions)
            {
                StoreTransaction(Block, Transaction);
            }

            // Completed
            return true;
        }

        // Checks if we have a block with this hash stored
        internal bool IsBlockStored(string Hash)
        {
            // Check if database is started
            if (!Database.Started) return false;

            // Count the instances of this block hash
            var Count = Database.BlockRepository.Count(new ValueList
            {
                ["hash"] = Hash
            });
            return Count > 0;
        }

        #endregion

        #region Utilities

        // Gets how many generated units the last block had
        internal ulong GetAlreadyGeneratedUnits()
        {
            // Query the database
            var Result = Database.BlockRepository.FindTopBlock();

            // If there are no matching entries, return 0
            if (Result == null) return 0;

            // Otherwise return the last known amount
            else return Result.AlreadyGeneratedUnits;
        }

        // Gets or generates a network genesis block
        internal void CacheGenesisBlock()
        {
            // First, attempt to get the genesis block from the database
            if (!TryGetBlock(0, out Block Genesis))
            {
                // Genesis block wasn't found, generate it
                Logger?.Warning($"Genesis block was not found, generating a new one...");

                // If the genesis block is not found, create a new one
                Genesis = new Block(this)
                {
                    MajorVersion = Constants.BLOCK_MAJOR_VERSION_0 , //TODO: if Testnet, then Block V0, else if MainNet, then Block V1 - to be tested
                    MinorVersion = Constants.BLOCK_MINOR_VERSION,
                    Timestamp = 0,
                    Nonce = 70, 
                    Height = 0
                };

                // Set base transaction
                Genesis.SetBaseTransaction(new Transaction(Globals.CURRENCY_GENESIS_TRANSACTION));

                // Store this block in the database
                StoreBlock(Genesis);

                Logger?.Debug($"Genesis block created: {Genesis.Hash}");
            }
            Logger?.Debug($"Genesis block found: {Genesis.Hash}");

            // Assign cached genesis block to this block
            this.Genesis = Genesis;
        }

        // Builds a list of block hashes based on sparse chain data
        internal string[] BuildSparseChain(uint BlockIndex = 0)
        {
            // TODO - In core source, there is a comment listing the following:
            // IDs of the first 10 blocks are sequential, next goes with pow(2,n) offset,
            // like 2, 4, 8, 16, 32, 64 and so on, and the last one is always genesis block
            // ...
            // BUT, in testing, it's more the following:
            // Last hash is always the genesis hash, but the previous hashes follow the
            // scheme listed above... As in, there are no sequential first 10 blocks. :t_shrug:

            // Create a list of strings to add to for our output
            List<string> Hashes = new List<string>();

            // Use synced height if not specified, this is the starting index
            if (BlockIndex < 0) BlockIndex = Height;

            // Starting at index of 1 (one after genesis), double until we reach the starting index
            for (uint Height = 0; Height < BlockIndex; Height *= 2)
            {
                // Get info for the block at this height (height is index + 1)
                if (!TryGetBlock(Height + 1, out Block Block)) break;

                // Add the block hash for this height
                Hashes.Add(Block.Hash);
            }

            // Add genesis block hash
            // TODO - Store genesis info we need for sync on first call, since it doesn't change
            TryGetBlock(0, out Block Genesis);
            Hashes.Add(Genesis.Hash);

            // Return the list as a string array
            return Hashes.ToArray();
        }

        #endregion

        #endregion
    }
}
