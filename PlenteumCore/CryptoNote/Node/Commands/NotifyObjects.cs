//
// Copyright (c) 2018-2019 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System;
using System.Linq;
using Plenteum.CryptoNote.Blockchain;

namespace Plenteum.CryptoNote
{
    public sealed partial class Node
    {
        // A notify chain packet was received
        private void HandleNotifyObjects(Peer Peer, Packet Packet)
        {
            // Check if this peer is validated
            if (!Peer.Validated)
            {
                throw new InvalidOperationException($"Received a {Packet.Type} packet from non-validated peer {Peer.Address}:{Peer.Port}, killing connection...");
                //ToDo: Close COnnection and Drop Peer
            }

            //TODO: Handle Tx Pool 
            if (Packet.Flag == PacketFlag.REQUEST)
            {

            }
            else 
            { 

            }

        }

        // Sends a notify chain request packet
        private void RequestNotifyObjects(Peer Peer)
        {
            // Check if this peer is validated
            if (!Peer.Validated) return;

            // Construct a request packet
            var Request = new Packet(PacketType.NOTIFY_TX_POOL_REQUEST, PacketFlag.REQUEST, false)
            {
                //["block_ids"] = GetSparseChain()
            };

            // Send our request
            Peer.SendMessage(Request);
        }
    }
}
