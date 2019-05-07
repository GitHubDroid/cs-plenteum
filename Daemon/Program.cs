//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Blockchain;

namespace Daemon
{
    class Program
    {
        // Set port to global default
        static int Port = Globals.P2P_DEFAULT_PORT;

        // Application entry point
        static void Main(string[] args)
        {
            // Parse commandline arguments
            if (args.Length >= 1) Port = int.Parse(args[0]);

            // Create a daemon connection
            Daemon Daemon = new Daemon(Port);

            // Start daemon
            Daemon.Start();
        }
    }
}
