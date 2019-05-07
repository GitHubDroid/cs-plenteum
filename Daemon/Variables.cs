//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Blockchain;
using Canti.Blockchain.P2P;
using Canti.Utilities;

namespace Daemon
{
    partial class Daemon
    {
        // P2p server
        private static Server Server;

        // Console logger
        private static Logger Logger;

        // Protocol handler
        private static IProtocol Protocol;

        // Database handler
        private static IStorage Database;

        // True if daemon is running
        public bool Running = false;
    }
}
