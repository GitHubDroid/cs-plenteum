//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System;
using Canti.Blockchain.P2P;
using Canti.Utilities;

namespace Daemon
{
    partial class Daemon
    {
        // Custom incoming data handling
        private static void DataReceived(object sender, EventArgs e)
        {
            Packet Packet = (Packet)sender;
            //Logger.Log(Level.DEBUG, "Received packet from {0}: {1}", Packet.Peer.Address, Encoding.ByteArrayToHexString(Packet.Data));
        }

        // Custom outgoing data handling
        private static void DataSent(object sender, EventArgs e)
        {
            Packet Packet = (Packet)sender;
            //Logger.Log(Level.DEBUG, "Sent packet to {0}: {1}", Packet.Peer.Address, Encoding.ByteArrayToHexString(Packet.Data));
        }

        // An error was received
        private static void ServerError(object sender, EventArgs e)
        {
            Logger.Log(Level.ERROR, "Server error: {0}", (string)sender);
        }

        // Custom peer connected handling
        private static void PeerConnected(object sender, EventArgs e)
        {
            PeerConnection Peer = (PeerConnection)sender;
            Logger.Log(Level.DEBUG, "Peer connection formed with {0}", Peer.Address);
        }

        // Custom peer disconnected handling
        private static void PeerDisconnected(object sender, EventArgs e)
        {
            PeerConnection Peer = (PeerConnection)sender;
            Logger.Log(Level.DEBUG, "Peer connection lost with {0}", Peer.Address);
        }

        // Custom server start handling
        private static void ServerStarted(object sender, EventArgs e)
        {
            Server Server = (Server)sender;
            Logger.Log(Level.INFO, "Server started on port {0}, peer ID of {1}", Server.Port, Server.PeerId);
        }

        // Custom server stopped handling
        private static void ServerStopped(object sender, EventArgs e)
        {
            Server Server = (Server)sender;
            Logger.Log(Level.INFO, "Server stopped", Server.Port, Server.PeerId);
        }
    }
}
