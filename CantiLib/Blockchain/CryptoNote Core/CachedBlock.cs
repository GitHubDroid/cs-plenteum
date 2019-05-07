//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Blockchain.Crypto;
using Canti.Blockchain.Crypto.Keccak;
using Canti.Data;
using System;
using System.Collections.Generic;

namespace Canti.Blockchain
{
    // DEBUG / INCOMPLETE
    public class CachedBlock
    {
        // Contains the given block data
        Block Block;

        // Entry point, caches a given block
        public CachedBlock(Block Block)
        {
            this.Block = Block;
        }

        // Returns block's hash
        public string GetBlockHash()
        {
            // Check block version and append parent block data if needed
            if (Globals.BLOCK_MAJOR_VERSION_2 <= Block.Header.MajorVersion)
            {
                //Block parentBlock = getParentBlockHashingBinaryArray(false);
                //blockBinaryArray.insert(blockBinaryArray.end(), parentBlock.begin(), parentBlock.end());
            }

            // Convert block data to a byte array
            byte[] BlockData = GetBlockHashingByteArray();

            // Get result in the form of a cn_fast_hash/keccak1600 hash
            byte[] Output = TreeHash.cn_fast_hash(BlockData, 32, new byte[32]);
            Console.WriteLine("Output after cn_fast_hash: " + Encoding.ByteArrayToHexString(Output) + "\n");

            // Return hex representation of output byte array
            return Encoding.ByteArrayToHexString(Output);
        }

        // Returns block data as a byte array
        byte[] GetBlockHashingByteArray()
        {
            // Create a binary serializer
            byte[] Output = new byte[0];

            // Set output array to byte array representation of the block header
            //Output = Encoding.ObjectToByteArray(Block.Header);
            Output = BinaryOutputSerializer.SerializeObjectAsBinary(Block.Header);
            //blockHashingBinaryArray = BinaryArray();
            //auto& result = blockHashingBinaryArray.get();
            //toBinaryArray(static_cast<const BlockHeader&>(block), result)
            Console.WriteLine("Output w/Block Header: " + Encoding.ByteArrayToHexString(Output) + "\n");

            // Append the transaction tree hash to output array
            Output = Encoding.AppendToByteArray(Encoding.HexStringToByteArray(GetTransactionTreeHash()), Output);
            //const auto& treeHash = getTransactionTreeHash();
            //result.insert(result.end(), treeHash.data, treeHash.data + 32);
            Console.WriteLine("Output w/Transaction Tree Hash: " + Encoding.ByteArrayToHexString(Output) + "\n");

            // Append the transactions to output array
            if (Block.TransactionHashes != null)
            {
                // Add transaction count
                Output = Encoding.AppendToByteArray(Encoding.IntegerToByteArray(Block.TransactionHashes.Length), Output);

                // Add all transaction hashes
                foreach (string Hash in Block.TransactionHashes)
                    Output = Encoding.AppendToByteArray(Encoding.HexStringToByteArray(Hash), Output);
            }
            else Output = Encoding.AppendToByteArray(Encoding.IntegerToByteArray(0), Output);
            //auto transactionCount = Common::asBinaryArray(Tools::get_varint_data(block.transactionHashes.size() + 1));
            //result.insert(result.end(), transactionCount.begin(), transactionCount.end());
            Console.WriteLine("Output w/Transaction Hashes: " + Encoding.ByteArrayToHexString(Output) + "\n");

            // Return completed output array
            return Output;
        }

        // Returns a hash representation of the block's transaction tree
        string GetTransactionTreeHash()
        {
            // Create a list of hashes
            List<string> TransactionHashes = new List<string>
            {
                // Add block's base transaction
                //Encoding.ByteArrayToHexString(TreeHash.cn_fast_hash(Encoding.ObjectToByteArray(Block.BaseTransaction), 32, new byte[32]))
                Encoding.ByteArrayToHexString(Keccak.keccak1600(BinaryOutputSerializer.SerializeObjectAsBinary(Block.BaseTransaction)))
            };

            // Add each transaction hash in the block
            if (Block.TransactionHashes != null)
                foreach (string Hash in Block.TransactionHashes)
                    TransactionHashes.Add(Hash);

            // Return tree hashed output
            return TreeHash.Hash(TransactionHashes.ToArray(), Block.BaseTransaction.Hash);

            /*std::vector<Crypto::Hash> transactionHashes; // Create hash vector
            transactionHashes.reserve(block.transactionHashes.size() + 1); // Set vector size in memory
            transactionHashes.push_back(getObjectHash(block.baseTransaction)); // Add base transaction
            transactionHashes.insert(transactionHashes.end(), block.transactionHashes.begin(), block.transactionHashes.end()); // Insert each transaction hash from the block
            transactionTreeHash = Crypto::Hash();
            Crypto::tree_hash(transactionHashes.data(), transactionHashes.size(), transactionTreeHash.get());

            return transactionTreeHash.get();*/
        }
    }
}
