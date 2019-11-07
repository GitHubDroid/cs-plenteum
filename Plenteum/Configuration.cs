//
// Copyright (c) 2018-2019 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Plenteum;
using Plenteum.CryptoNote;
using System;
using static Plenteum.Utils;

namespace Plenteum
{
    partial class Daemon
    {
        // This is the network configuration we will pass to our node instance when creating it
        private static readonly NetworkConfig Configuration = new NetworkConfig
        {
            #region NETWORK

            NETWORK_ID = new byte[] {
                0xa2, 0x7d, 0x4b, 0x2c, 0xcf, 0x52, 0x37, 0x41, 
                0x35, 0xf9, 0x41, 0xa4, 0xc6, 0xa1, 0x43, 0xa2
            },

            ASCII_ART = "" +
                    "\n                                   \n" +
                    "          _________________________  \n" +
                    "         / _____________________  /| \n" +
                    "        / / ___________________/ / | \n" +
                    "       / / /| |               / /  | \n" +
                    "      / / / | |              / / . | \n" +
                    "     / / /| | |             / / /| | \n" +
                    "    / / / | | |            / / / | | \n" +
                    "   / / /  | | |           / / /| | | \n" +
                    "  / /_/___|_|_|__________/ / / | | | \n" +
                    " /________________________/ /  | | | \n" +
                    " | | |    | | |_________| | |__| | | \n" +
                    " | | |    | |___________| | |____| | \n" +
                    " | | |   / / ___________| | |_  / / \n" +
                    " | | |  / / /           | | |/ / / \n" +
                    " | | | / / /            | | | / / \n" +
                    " | | |/ / /             | | |/ / \n" +
                    " | | | / /  Plenteum    | | ' / \n" +
                    " | | |/_/_______________| |  / \n" +
                    " | |____________________| | / \n" +
                    " |________________________|/ \n",

        SEED_NODES = new PeerCandidate[]
            {
                //new PeerCandidate("197.81.192.74", 44025)
                //new PeerCandidate("104.41.136.178", 44025), //testnet-2
                //new PeerCandidate("137.117.87.159", 44025) //Testnet-3
                new PeerCandidate("51.145.22.178", 44025) //TestNet-4
            },

            #endregion

            #region CURRENCY

            CURRENCY_NAME = "Plenteum",
            CURRENCY_DIFFICULTY_TARGET = 120,
            CURRENCY_TOTAL_SUPPLY = 21_000_000_000_00000000,
            CURRENCY_EMISSION_FACTOR = 22,
            CURRENCY_GENESIS_REWARD = 525_000_000_00000000,
            CURRENCY_GENESIS_TRANSACTION = "011401ff00018080d5d58c8fa15d02cb36419ab0e1708b9d5f974a2f134d211a367cd1d29ca4be6f1b38ec1864faab2101e0e36382a0d8c01c856563e6d8f59591bb7fae4f91fb2fce431e11fe6c896348",

            #endregion

            #region STORAGE

            DATABASE_TYPE = DatabaseType.IBOXDB,
            LOCAL_STORAGE_DIRECTORY = GetAppDataPath("Plenteum"),

            #endregion

            #region P2P

            P2P_DEFAULT_PORT = 8090,
            P2P_WORKERS = 4,
            P2P_CURRENT_VERSION = 5,
            P2P_MINIMUM_VERSION = 4,
            P2P_MIN_PEER_CONNECTIONS = 4,
            P2P_MAX_PEER_CONNECTIONS = 50,
            P2P_DISCOVERY_ENABLED = true,
            P2P_DISCOVERY_INTERVAL = 5,
            P2P_DISCOVERY_TIMEOUT = 1200,
            P2P_CONNECTION_TIMEOUT = 2000,
            P2P_POLLING_INTERVAL = 100,
            P2P_TIMED_SYNC_INTERVAL = 60,
            P2P_HANDSHAKE_TIME_DELTA = 2,
            P2P_LAST_SEEN_DELTA = 600, // TODO - <-- Possibly remove

            #endregion

            #region API

            API_ENABLED = true,
            API_PASSWORD = "54321",
            API_DEFAULT_PORT = 8091,
            API_WORKERS = 5,
            API_CURRENT_VERSION = 1,
            API_MINIMUM_VERSION = 0,

            #endregion

            #region Logger

            LOG_FILE = "Plenteum.log",
            LOG_LEVEL = LogLevel.MAX,
            INFO_COLOR = ConsoleColor.White,
            IMPORTANT_COLOR = ConsoleColor.Green,
            DEBUG_COLOR = ConsoleColor.DarkGray,
            WARNING_COLOR = ConsoleColor.Yellow,
            ERROR_COLOR = ConsoleColor.Red,

            #endregion
        };
    }
}
