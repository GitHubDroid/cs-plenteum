//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Data;
using System;
using System.Collections.Generic;

namespace Canti.Blockchain
{
    // DEBUG / INCOMPLETE
    public class Currency
    {
        // Generates the currency's genesis block
        public static CachedBlock GenerateGenesisBlock()
        {
            // Create a new block from template
            Block GenesisBlock = new Block();

            // Set genesis coinbase transaction hex
            GenesisBlock.BaseTransaction = new Transaction
            {
                Hash = "42694232c5b04151d9e4c27d31ec7a68ea568b19488cfcb422659a07a0e44dd5",
                Extra = new byte[] { 0x01, 0x42, 0x69, 0x42, 0x32, 0xC5, 0xB0, 0x41, 0x51, 0xD9, 0xE4, 0xC2, 0x7D, 0x31, 0xEC, 0x7A, 0x68, 0xEA, 0x56, 0x8B, 0x19, 0x48, 0x8C, 0xFC, 0xB4, 0x22, 0x65, 0x9A, 0x07, 0xA0, 0xE4, 0x4D, 0xD5 },
                Outputs = new Output[]
                {
                    new Output
                    {
                        Amount = 2980232,
                        Key = "9b2e4c0281c0b02e7c53291a94d1d0cbff8883f8024f5142ee494ffbbd088071"
                    }
                },
                Inputs = new Input[0],
                Signatures = new string[0],
                UnlockTime = 10
            };
            //Encoding.DecodeObject<Transaction>(Encoding.HexStringToByteArray(Globals.GENESIS_COINBASE_TX_HEX));
            //GenesisBlock.BaseTransaction = Globals.NULL_HASH;
            GenesisBlock.Height = 1;

            //string genesisCoinbaseTxHex = Globals.GENESIS_COINBASE_TX_HEX;
            //byte[] minerTxBlob = Encoding.HexStringToByteArray(Globals.GENESIS_COINBASE_TX_HEX);
            /*fromHex(genesisCoinbaseTxHex, minerTxBlob) &&
            fromBinaryArray(genesisBlockTemplate.baseTransaction, minerTxBlob);*/

            /*if (!r)
            {
                logger(ERROR, BRIGHT_RED) << "failed to parse coinbase tx from hard coded blob";
                return false;
            }*/

            // Create block header
            Block.BlockHeader Header = new Block.BlockHeader();
            Header.MajorVersion = Globals.BLOCK_MAJOR_VERSION_1;
            Header.MinorVersion = Globals.BLOCK_MINOR_VERSION_0;
            Header.Timestamp = Globals.GENESIS_BLOCK_TIMESTAMP;
            Header.Nonce = 70;
            if (Globals.TESTNET) Header.Nonce++;

            // Assign header to block
            GenesisBlock.Header = Header;

            //cachedGenesisBlock.reset(new CachedBlock(genesisBlockTemplate));
            
            // Return created genesis block
            return new CachedBlock(GenesisBlock);
        }
    }
}
