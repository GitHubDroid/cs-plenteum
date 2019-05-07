//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Blockchain;
using Canti.Utilities;

namespace Daemon
{
    partial class Daemon
    {
        // Connects to a random seed node, returns false if no connections could be made
        private bool ConnectToSeedNode()
        {
            // Randomize seed node list
            Globals.SEED_NODES.Shuffle();

            // Loops through list and connects to a node
            for (int i = 0; i < Globals.SEED_NODES.Count; i++)
            {
                // Select a node from the list
                Connection Node = Globals.SEED_NODES[i];

                // Attempt to connect to node
                if (Node.TestConnection() && Server.Connect(Node)) return true;
            }

            // No node connection could be established
            return false;
        }
    }
}
