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
        private void HandleNotifyChain(Peer Peer, Packet Packet)
        {
            // Check if this peer is validated
            if (!Peer.Validated)
            {
                throw new InvalidOperationException($"Received a {Packet.Type} packet from non-validated peer {Peer.Address}:{Peer.Port}, killing connection...");
                //ToDo: Close COnnection and Drop Peer
            }
            if (Packet.Flag == PacketFlag.REQUEST && Packet.Type == PacketType.NOTIFY_CHAIN_RESPONSE)
            {
                var block_ids = (string)Packet.Body["m_block_ids"];
                var start_height = (int)Packet.Body["start_height"];
                var total_height = (int)Packet.Body["total_height"];

                //At this point we know we have the genesis block, so we need to start sync'ing from block number 2
                if (!string.IsNullOrEmpty(block_ids))
                {
                    var hashes = block_ids.SplitToHashes();
                    if (hashes.Count() == (total_height - start_height))
                    { 
                        //we have received the expected number of hashes... 

                        //now, we need to get some consensus

                    }
                }
            }
            else
            {
                var block_ids = (string)Packet.Body["block_ids"];
                if (!string.IsNullOrEmpty(block_ids))
                {
                    var hashes = block_ids.SplitToHashes();
                    //check if array empty
                    if (!hashes.Any())
                    {
                        throw new InvalidOperationException($"Failed to handle notify chain response, no return value");
                        //ToDo: Close COnnection and Drop Peer
                    }
                    //check against genesis block
                    if (hashes.Last() != Blockchain.GetBlockHash(0))
                    {
                        throw new InvalidOperationException($"Failed to handle notify chain response due to genesis block mismatch: {hashes.Last()} != {Blockchain.GetBlockHash(1)}");
                        //ToDo: Close COnnection and Drop Peer
                    }

                    //Get our top block we have for this node
                    Block Block = null;
                    bool topBlockFound = Blockchain.TryGetTopBlock(out Block);
                    //create a new request for batch of blocks starting at our topBlock
                    if (topBlockFound && Block != null && Block.Hash != Constants.NULL_HASH)
                    {
                        //fetch the Blocks from the peer
                    }
                    //NOTIFY_RESPONSE_CHAIN_ENTRY
                    //Post that request to the peers

                    // TODO - continue handle sync stuff

                }
                else
                {
                    throw new InvalidOperationException($"Failed to handle notify chain response, no return value");
                    //ToDo: Close COnnection and Drop Peer
                }
            }
        }

        // Sends a notify chain request packet
        private void RequestNotifyChain(Peer Peer)
        {
            // Check if this peer is validated
            if (!Peer.Validated) return;

            // Construct a request packet
            var Request = new Packet(PacketType.NOTIFY_CHAIN_REQUEST, PacketFlag.REQUEST, false)
            {
                ["block_ids"] = GetSparseChain()
            };

            // Send our request
            Peer.SendMessage(Request);
        }
    }
}
