//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Data;
using Canti.Utilities;
using System;
using System.Collections.Generic;

namespace Canti.Blockchain
{
    public class Globals
    {
        #region CONFIGURATION
        /* NETWORK */
        public static bool TESTNET = false;
        public static string VERSION = "v0.0.1"; // The version of your software you are currently on
        public static string TICKER = "TRTL"; // What is your coin ticker, e.g. Bitcoin = BTC, Monero = XMR
        public static string COIN_NAME = "TurtleCoin"; // The name of your coin
        public static string NETWORK_ID = Encoding.ByteArrayToString(new byte[] { 0xb5, 0x0c, 0x4a, 0x6c, 0xcf, 0x52, 0x57, 0x41, 0x65, 0xf9, 0x91, 0xa4, 0xb6, 0xc1, 0x43, 0xe9 }); // TurtleCoin
        //public static string NETWORK_ID = Encoding.ByteArrayToString(new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xf0 }); // Athena Network

        /* CURRENCY */
        public const uint CRYPTONOTE_DISPLAY_DECIMAL_POINT = 2; // How many decimal places the currency has
        public const ulong GENESIS_BLOCK_REWARD = 0; // Premine amount
        public const string GENESIS_COINBASE_TX_HEX = "010a01ff000188f3b501029b2e4c0281c0b02e7c53291a94d1d0cbff8883f8024f5142ee494ffbbd088071210142694232c5b04151d9e4c27d31ec7a68ea568b19488cfcb422659a07a0e44dd5";
        public const ulong GENESIS_BLOCK_TIMESTAMP = 1512800692;

        /* DAEMON */
        public static Level LOG_LEVEL = Level.INFO; // Default log level
        public static string LOG_FILE = null; // Default log file (null = no log file)
        /*public static List<Connection> SEED_NODES = new List<Connection> // Network seed nodes
        {
            new Connection("206.189.142.142", 11897, ""), // Rock
            new Connection("145.239.88.119", 11999, ""),  // Cision
            new Connection("142.44.242.106", 11897, ""),  // Tom
            new Connection("165.227.252.132", 11897, "")  // iburnmycd
        }; // TurtleCoin*/
        public static List<Connection> SEED_NODES = new List<Connection> // Network seed nodes
        {
            new Connection("51.15.142.102", 12000, ""),   // Athena-0
            new Connection("51.15.137.77", 12000, ""),    // Athena-1
            new Connection("165.227.252.132", 12000, ""), // Community Node
            new Connection("95.179.138.105", 12000, "")   // Community Node
        }; // Athena Network

        /* CLI WALLET */
        public static string CLI_WALLET_NAME = "zedwallet++"; // The name of the CLI Wallet
        public static ulong ADDRESS_PREFIX = 0x3bbb1d; // What prefix does your address start with - see https://cryptonotestarter.org/tools.html (This one == TRTL)

        /* P2P DEFAULTS */
        public const int P2P_DEFAULT_PORT = 8090;

        /* RPC DEFAULTS */
        public const int RPC_DEFAULT_PORT = 8091;

        /* LEVIN PROTOCOL */
        public const ulong LEVIN_SIGNATURE = 0x0101010101012101UL; // Bender's Nightmare
        public static byte LEVIN_VERSION = LevinProtocol.LEVIN_PROTOCOL_VER_1;
        public const int LEVIN_DEFAULT_TIMEOUT = 0;
        public const int LEVIN_MAX_PACKET_SIZE = 100000000; // 100 MB
        public const int LEVIN_COMMANDS_BASE = 1000; // Levin protocol packet command codes are added to this value

        /* PORTABLE STORAGE */
        public const uint STORAGE_SIGNATUREA = 0x01011101; // Bender's Nightmare (Part 1)
        public const uint STORAGE_SIGNATUREB = 0x01020101; // Bender's Nightmare (Part 2) 
        public const byte STORAGE_FORMAT_VERSION = 1;

        /* CRYPTONOTE PROTOCOL (LEVIN) */
        public const int CRYPTONOTE_COMMANDS_BASE = 2000; // Cryptonote protocol packet command codes are added to this value

        /* BLOCKCHAIN */
        public const byte BLOCK_MAJOR_VERSION_1 = 1;
        public const byte BLOCK_MAJOR_VERSION_2 = 2;
        public const byte BLOCK_MAJOR_VERSION_3 = 3;
        public const byte BLOCK_MAJOR_VERSION_4 = 4;
        public const byte BLOCK_MINOR_VERSION_0 = 0;
        public const byte BLOCK_MINOR_VERSION_1 = 1;
        #endregion

        #region DO NOT EDIT
        /* HASHING */
        public const string NULL_HASH = "0000000000000000000000000000000000000000000000000000000000000000";

        /* DAEMON & BLOCK STORAGE */
        public static IBlockchainCache[] DAEMON_CHAIN_LEAVES = new IBlockchainCache[] { };
        public static uint DAEMON_BLOCK_HEIGHT = 0;
        public static string DAEMON_TOP_ID = string.Empty;
        public static PeerlistEntry[] DAEMON_PEERLIST = new PeerlistEntry[0];
        #endregion
    }
}
