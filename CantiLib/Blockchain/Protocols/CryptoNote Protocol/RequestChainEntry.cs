//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Blockchain.Crypto;
using Canti.Utilities;

namespace Canti.Blockchain.Commands
{
    public class RequestChainEntry
    {
        // Command ID
        public const int Id = Globals.CRYPTONOTE_COMMANDS_BASE + 7;

        // Outgoing request structure
        public struct Request : ICommandRequestBase
        {
            // Variables
            public uint StartHeight;
            public uint TotalHeight;
            public string[] MBlockIds;

            // Serializes request data into a byte array
            public byte[] Serialize()
            {
                // Create a portable storage
                PortableStorage Storage = new PortableStorage();

                // Add entries
                Storage.AddEntry("start_height", StartHeight);
                Storage.AddEntry("total_height", TotalHeight);
                Storage.AddEntryAsBinary("m_block_ids", MBlockIds);

                // Return serialized byte array
                return Storage.Serialize();
            }

            // Deseriaizes response data
            public static Request Deserialize(byte[] Data)
            {
                // Deserialize data
                PortableStorage Storage = new PortableStorage();
                Storage.Deserialize(Data);

                // Populate and return new response
                return new Request
                {
                    StartHeight = (uint)Storage.GetEntry("start_height"),
                    TotalHeight = (uint)Storage.GetEntry("total_height"),
                    MBlockIds = Hashing.DeserializeHashArray((string)Storage.GetEntry("m_block_ids"))
                };
            }
        }

        // Process incoming command instance
        public static void Invoke(LevinProtocol Context, LevinPeer Peer, Command Command)
        {
            // Command is a request
            if (!Command.IsResponse)
            {
                // Deserialize request
                Request Request = Request.Deserialize(Command.Data);

                // debug
                Context.Logger?.Log(Level.DEBUG, "[IN] Received \"Notify Request Chain Entry\" Request:");
                Context.Logger?.Log(Level.DEBUG, "- Response Requested: {0}", !Command.IsNotification);
                Context.Logger?.Log(Level.DEBUG, "- Start Height: {0}", Request.StartHeight);
                Context.Logger?.Log(Level.DEBUG, "- Total Height: {0}", Request.TotalHeight);
                Context.Logger?.Log(Level.DEBUG, "- M Block IDs:");
                for (int i = 0; i < Request.MBlockIds.Length; i++)
                    Context.Logger?.Log(Level.DEBUG, "  - [{0}]: {1}", i, Request.MBlockIds[i]);

                // TODO: Do something with request data
            }
        }
    }
}
