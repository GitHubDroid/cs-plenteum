//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Data;
using Canti.Utilities;
using System;

namespace Canti.Blockchain.Commands
{
    public class RequestPeerId
    {
        // Command ID
        public const int Id = Globals.LEVIN_COMMANDS_BASE + 6;

        // Outgoing request structure
        public struct Request : ICommandRequestBase
        {
            // Serializes request data into a byte array
            public byte[] Serialize()
            {
                // No data is needed for this request
                return new byte[0];
            }
        }

        // Incoming response structure
        public struct Response : ICommandResponseBase<Response>
        {
            // Variables
            public ulong PeerId { get; set; }

            // Serializes response data
            public byte[] Serialize()
            {
                // Create a portable storage
                PortableStorage Storage = new PortableStorage();

                // Add entries
                Storage.AddEntry("my_id", PeerId);

                // Return serialized byte array
                return Storage.Serialize();
            }

            // Deseriaizes response data
            public static Response Deserialize(byte[] Data)
            {
                // Deserialize data
                PortableStorage Storage = new PortableStorage();
                Storage.Deserialize(Data);

                // Populate and return new response
                return new Response
                {
                    PeerId = (ulong)Storage.GetEntry("my_id")
                };
            }
        }

        // Process incoming command instance
        public static void Invoke(LevinProtocol Context, LevinPeer Peer, Command Command)
        {
            // Command is a request
            if (!Command.IsResponse)
            {
                // debug
                Context.Logger?.Log(Level.DEBUG, "[IN] Received \"Request Peer ID\" Request:");
                Context.Logger?.Log(Level.DEBUG, "- Response Requested: {0}", !Command.IsNotification);

                // TODO: Do something with request data

                // TODO: Do some processing in here, make sure the packet isn't a notification for some reason,
                //       make sure peer isn't duplicate, etc.

                // Create a response
                Response Response = new Response
                {
                    PeerId = Context.Server.PeerId
                };

                // debug
                Context.Logger?.Log(Level.DEBUG, "[OUT] Sending \"Request Peer ID\" Response:");
                Context.Logger?.Log(Level.DEBUG, "- Peer ID: {0}", Response.PeerId);

                // Reply with response
                Context.Reply(Peer, Id, Response.Serialize(), true);
            }

            // Command is a response
            else
            {
                // Deserialize response
                Response Response = Response.Deserialize(Command.Data);

                // debug
                Context.Logger?.Log(Level.DEBUG, "[IN] Received \"Request Peer ID\" Response:");
                Context.Logger?.Log(Level.DEBUG, "- Response Requested: {0}", !Command.IsNotification);
                Context.Logger?.Log(Level.DEBUG, "- Peer ID: {0}", Response.PeerId);

                // TODO: Do something with response data
            }
        }
    }
}
