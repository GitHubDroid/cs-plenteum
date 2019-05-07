//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Blockchain;
using Canti.Blockchain.Commands;
using Canti.Blockchain.P2P;
using Canti.Data;
using Canti.Utilities;
using System;
using System.Collections.Generic;

namespace Daemon
{
    // DEBUG / INCOMPLETE
    // Network daemon initializing class
    partial class Daemon
    {
        // Entry point
        public Daemon(int Port = 0)
        {
            // Create logger
            Logger = new Logger(Globals.LOG_FILE);
            Logger.LogLevel = Level.DEBUG;
            Logger.Start();

            // Create server
            Server = new Server();
            Server.Logger = Logger;

            // Bind event handlers
            Server.OnStart += ServerStarted;
            Server.OnStop += ServerStopped;
            Server.OnDataReceived += DataReceived;
            Server.OnDataSent += DataSent;
            Server.OnError += ServerError;
            Server.OnPeerConnected += PeerConnected;

            // Subscribe a protocol handler
            Protocol = new LevinProtocol(Server, Globals.NETWORK_ID);

            // Start server
            if (Port != 0) Server.Start(Port);
            else Server.Start(Globals.P2P_DEFAULT_PORT);

            // Set as running
            Running = true;
        }

        // Run daemon
        public void Start()
        {
            // Check if running
            if (!Running) return;

            /*
             * 
             * This is basically all debugging still
             * 
             * Test order:
             * 1. Connect to a Node
             * 2. Handshake with that node
             * 3. Grab height from that node's handshake response
             * 4. Pass a Notify Request Chain Entry packet
             * 5. Record response
             * 
             */

            // Check for genesis block
            if (Globals.DAEMON_BLOCK_HEIGHT < 1 || Globals.DAEMON_TOP_ID == string.Empty)
            {
                // No genesis block was found
                Logger?.Log(Level.WARNING, "No genesis block was found, generating now...");

                // Generate genesis block
                CachedBlock Genesis = Currency.GenerateGenesisBlock();

                // Set globals
                Globals.DAEMON_BLOCK_HEIGHT = 1;
                Globals.DAEMON_TOP_ID = Genesis.GetBlockHash(); // Get actual block ID later pls

                // Log output
                Logger?.Log(Level.INFO, "Genesis block formed, new top ID: " + Globals.DAEMON_TOP_ID);
            }

            // Enter into a loop
            int MenuSelection = -1;
            while (MenuSelection != 10 && Running)
            {
                // Connect to a seed node
                if (MenuSelection == 0)
                {
                    Logger.Log(Level.INFO, "Connecting to a seed node");
                    if (!ConnectToSeedNode()) Logger.Log(Level.ERROR, "No seed nodes are available");
                }

                // Manually connect to a peer
                else if (MenuSelection == 1)
                {
                    Logger.Log(Level.INFO, "Enter a URL:");
                    string Url = Console.ReadLine();
                    Logger.Log(Level.INFO, "Enter a port:");
                    int Port = int.Parse(Console.ReadLine());

                    // Select a node from the list
                    Connection Node = new Connection(Url, Port, "");

                    // Attempt to connect to node
                    if (!Server.Connect(Node)) Logger.Log(Level.ERROR, "Failed to connect to {0}:{1}", Url, Port);
                }

                // Show peer list
                else if (MenuSelection == 2)
                {
                    Server.Prune();
                    string Peers = "";
                    List<PeerConnection> PeerList = Server.GetPeerList();
                    foreach (PeerConnection Peer in PeerList) Peers += Peer.Address + " ";
                    Logger.Log(Level.DEBUG, "Peers:");
                    Logger.Log(Level.DEBUG, Peers);
                }

                // Broadcast a test packet (handshake)
                else if (MenuSelection == 3)
                {
                    // Create a response
                    Handshake.Request Request = new Handshake.Request
                    {
                        NodeData = new NodeData()
                        {
                            NetworkId = Globals.NETWORK_ID,
                            Version = 3,
                            Port = 8090,
                            LocalTime = GeneralUtilities.GetTimestamp(),
                            PeerId = Server.PeerId
                        },
                        PayloadData = new CoreSyncData()
                        {
                            CurrentHeight = Globals.DAEMON_BLOCK_HEIGHT,
                            TopId = Globals.DAEMON_TOP_ID
                        }
                    };

                    // Get body bytes
                    byte[] BodyBytes = Request.Serialize();

                    // Create a header
                    BucketHead2 Header = new BucketHead2
                    {
                        Signature = Globals.LEVIN_SIGNATURE,
                        ResponseRequired = false,
                        PayloadSize = (ulong)BodyBytes.Length,
                        CommandCode = (uint)Handshake.Id,
                        ProtocolVersion = Globals.LEVIN_VERSION,
                        Flags = LevinProtocol.LEVIN_PACKET_REQUEST,
                        ReturnCode = LevinProtocol.LEVIN_RETCODE_SUCCESS
                    };

                    Logger?.Log(Level.DEBUG, "[OUT] Sending Handshake Request:");
                    Logger?.Log(Level.DEBUG, "- Node Data:");
                    Logger?.Log(Level.DEBUG, "  - Network ID: {0}", Encoding.StringToHexString(Request.NodeData.NetworkId));
                    Logger?.Log(Level.DEBUG, "  - Peer ID: {0}", Request.NodeData.PeerId);
                    Logger?.Log(Level.DEBUG, "  - Version: {0}", Request.NodeData.Version);
                    Logger?.Log(Level.DEBUG, "  - Local Time: {0}", Request.NodeData.LocalTime);
                    Logger?.Log(Level.DEBUG, "  - Port: {0}", Request.NodeData.Port);
                    Logger?.Log(Level.DEBUG, "- Core Sync Data:");
                    Logger?.Log(Level.DEBUG, "  - Current Height: {0}", Request.PayloadData.CurrentHeight);
                    Logger?.Log(Level.DEBUG, "  - Top ID: {0}", Encoding.StringToHexString(Request.PayloadData.TopId));

                    // Send notification
                    Server.Broadcast(Encoding.AppendToByteArray(BodyBytes, Header.Serialize()));
                }

                // Broadcast a test packet (handshake)
                else if (MenuSelection == 4)
                {
                    // Create a response
                    TimedSync.Request Request = new TimedSync.Request
                    {
                        PayloadData = new CoreSyncData()
                        {
                            CurrentHeight = Globals.DAEMON_BLOCK_HEIGHT,
                            TopId = Globals.DAEMON_TOP_ID
                        }
                    };

                    // Get body bytes
                    byte[] BodyBytes = Request.Serialize();

                    // Create a header
                    BucketHead2 Header = new BucketHead2
                    {
                        Signature = Globals.LEVIN_SIGNATURE,
                        ResponseRequired = false,
                        PayloadSize = (ulong)BodyBytes.Length,
                        CommandCode = (uint)TimedSync.Id,
                        ProtocolVersion = Globals.LEVIN_VERSION,
                        Flags = LevinProtocol.LEVIN_PACKET_REQUEST,
                        ReturnCode = LevinProtocol.LEVIN_RETCODE_SUCCESS
                    };

                    Logger?.Log(Level.DEBUG, "[OUT] Sending Timed Sync Request:");
                    Logger?.Log(Level.DEBUG, "- Core Sync Data:");
                    Logger?.Log(Level.DEBUG, "  - Current Height: {0}", Request.PayloadData.CurrentHeight);
                    Logger?.Log(Level.DEBUG, "  - Top ID: {0}", Encoding.StringToHexString(Request.PayloadData.TopId));

                    // Send notification
                    Server.Broadcast(Encoding.AppendToByteArray(BodyBytes, Header.Serialize()));
                }

                // Broadcast a test packet (ping)
                else if (MenuSelection == 5)
                {
                    // Create a response
                    Ping.Request Request = new Ping.Request { };

                    // Get body bytes
                    byte[] BodyBytes = Request.Serialize();

                    // Create a header
                    BucketHead2 Header = new BucketHead2
                    {
                        Signature = Globals.LEVIN_SIGNATURE,
                        ResponseRequired = false,
                        PayloadSize = (ulong)BodyBytes.Length,
                        CommandCode = (uint)RequestPeerId.Id,
                        ProtocolVersion = Globals.LEVIN_VERSION,
                        Flags = LevinProtocol.LEVIN_PACKET_REQUEST,
                        ReturnCode = LevinProtocol.LEVIN_RETCODE_SUCCESS
                    };

                    Logger?.Log(Level.DEBUG, "[OUT] Sending Request Peer ID Request");

                    // Send notification
                    Server.Broadcast(Encoding.AppendToByteArray(BodyBytes, Header.Serialize()));
                }

                // Broadcast a test packet (request peer id)
                else if (MenuSelection == 6)
                {
                    // Create a response
                    RequestPeerId.Request Request = new RequestPeerId.Request { };

                    // Get body bytes
                    byte[] BodyBytes = Request.Serialize();

                    // Create a header
                    BucketHead2 Header = new BucketHead2
                    {
                        Signature = Globals.LEVIN_SIGNATURE,
                        ResponseRequired = false,
                        PayloadSize = (ulong)BodyBytes.Length,
                        CommandCode = (uint)RequestPeerId.Id,
                        ProtocolVersion = Globals.LEVIN_VERSION,
                        Flags = LevinProtocol.LEVIN_PACKET_REQUEST,
                        ReturnCode = LevinProtocol.LEVIN_RETCODE_SUCCESS
                    };

                    Logger?.Log(Level.DEBUG, "[OUT] Sending Request Peer ID Request:");

                    // Send notification
                    Server.Broadcast(Encoding.AppendToByteArray(BodyBytes, Header.Serialize()));
                }

                // Broadcast a test packet (request chain)
                else if (MenuSelection == 7)
                {
                    // Create a response
                    RequestChain.Request Request = new RequestChain.Request
                    {
                        BlockIds = new string[]
                        {
                            ""
                        }
                    };

                    // Get body bytes
                    byte[] BodyBytes = Request.Serialize();

                    // Create a header
                    BucketHead2 Header = new BucketHead2
                    {
                        Signature = Globals.LEVIN_SIGNATURE,
                        ResponseRequired = false,
                        PayloadSize = (ulong)BodyBytes.Length,
                        CommandCode = (uint)RequestChain.Id,
                        ProtocolVersion = Globals.LEVIN_VERSION,
                        Flags = LevinProtocol.LEVIN_PACKET_REQUEST,
                        ReturnCode = LevinProtocol.LEVIN_RETCODE_SUCCESS
                    };

                    Logger?.Log(Level.DEBUG, "[OUT] Sending Request Chain Request:");
                    Logger?.Log(Level.DEBUG, "- Block IDs: None");

                    // Send notification
                    Server.Broadcast(Encoding.AppendToByteArray(BodyBytes, Header.Serialize()));
                }

                // Broadcast a test packet (request chain entry)
                else if (MenuSelection == 8)
                {
                    // Create a response
                    RequestChainEntry.Request Request = new RequestChainEntry.Request
                    {
                        StartHeight = Globals.DAEMON_BLOCK_HEIGHT,
                        TotalHeight = Globals.DAEMON_BLOCK_HEIGHT + 1,
                        MBlockIds = new string[]
                        {
                            Globals.DAEMON_TOP_ID
                        }
                    };

                    // Get body bytes
                    byte[] BodyBytes = Request.Serialize();

                    // Create a header
                    BucketHead2 Header = new BucketHead2
                    {
                        Signature = Globals.LEVIN_SIGNATURE,
                        ResponseRequired = false,
                        PayloadSize = (ulong)BodyBytes.Length,
                        CommandCode = (uint)RequestChainEntry.Id,
                        ProtocolVersion = Globals.LEVIN_VERSION,
                        Flags = LevinProtocol.LEVIN_PACKET_REQUEST,
                        ReturnCode = LevinProtocol.LEVIN_RETCODE_SUCCESS
                    };

                    Logger?.Log(Level.DEBUG, "[OUT] Sending Request Chain Entry Request:");
                    Logger?.Log(Level.DEBUG, "- Start Height: " + Globals.DAEMON_BLOCK_HEIGHT);
                    Logger?.Log(Level.DEBUG, "- Total Height: " + (Globals.DAEMON_BLOCK_HEIGHT + 1));
                    Logger?.Log(Level.DEBUG, "- M Block IDs: None");

                    // Send notification
                    Server.Broadcast(Encoding.AppendToByteArray(BodyBytes, Header.Serialize()));
                }

                // Broadcast a test packet (request tx pool)
                else if (MenuSelection == 9)
                {
                    // Create a response
                    RequestTxPool.Request Request = new RequestTxPool.Request
                    {
                        Txs = new string[0]
                    };

                    // Get body bytes
                    byte[] BodyBytes = Request.Serialize();

                    // Create a header
                    BucketHead2 Header = new BucketHead2
                    {
                        Signature = Globals.LEVIN_SIGNATURE,
                        ResponseRequired = false,
                        PayloadSize = (ulong)BodyBytes.Length,
                        CommandCode = (uint)RequestTxPool.Id,
                        ProtocolVersion = Globals.LEVIN_VERSION,
                        Flags = LevinProtocol.LEVIN_PACKET_REQUEST,
                        ReturnCode = LevinProtocol.LEVIN_RETCODE_SUCCESS
                    };

                    Logger?.Log(Level.DEBUG, "[OUT] Sending Request TX Pool Request:");
                    Logger?.Log(Level.DEBUG, "- TXs: None");

                    // Send notification
                    Server.Broadcast(Encoding.AppendToByteArray(BodyBytes, Header.Serialize()));
                }

                // Write menu
                Logger.Log(Level.INFO, "Menu:");
                Logger.Log(Level.INFO, " 0\tConnect to a Seed Node");
                Logger.Log(Level.INFO, " 1\tConnect to a Server");
                Logger.Log(Level.INFO, " 2\tShow Peer List");
                Logger.Log(Level.INFO, " 3\tTest 1001 Packet (Handhshake)");
                Logger.Log(Level.INFO, " 4\tTest 1002 Packet (Timed Sync)");
                Logger.Log(Level.INFO, " 5\tTest 1003 Packet (Ping)");
                Logger.Log(Level.INFO, " 6\tTest 1006 Packet (Request Peer ID)");
                Logger.Log(Level.INFO, " 7\tTest 2006 Packet (Request Chain)");
                Logger.Log(Level.INFO, " 8\tTest 2007 Packet (Request Chain Entry)");
                Logger.Log(Level.INFO, " 9\tTest 2008 Packet (Request TX Pool)");
                Logger.Log(Level.INFO, " 10\tExit");
                Logger.Log(Level.INFO, "Enter Selection:");

                // Get menu selection
                MenuSelection = int.Parse(Console.ReadLine());
            }

            // Stop daemon
            Stop();
        }

        public void Stop()
        {
            // Set to not running
            Running = false;

            // Close all connections
            Server?.Close();

            // Close logger
            Logger?.Stop();
        }
    }
}
