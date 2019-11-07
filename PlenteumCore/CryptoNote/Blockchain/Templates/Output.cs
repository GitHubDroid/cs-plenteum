//
// Copyright (c) 2018-2019 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System;
using static Plenteum.Utils;

namespace Plenteum.CryptoNote.Blockchain
{



    [Serializable]
    public class Output
    {
        // Output type (possibly always 02?)
        public byte Type { get; set; }

        // Amount in this output
        public ulong Amount { get; set; }

        // A unique key representing this output
        public string PublicKey { get; set; }

        // Returns a byte array representation of this output
        internal byte[] Serialize()
        {
            // Create an output buffer
            byte[] Buffer = new byte[0];

            // Add amount
            Buffer = Buffer.AppendVarInt(Amount);

            // Add type
            Buffer = Buffer.AppendVarInt(Type);

            // Add key
            Buffer = Buffer.AppendHexString(PublicKey);

            // Completed, return output buffer
            return Buffer;
        }

        // Deserializes an output from a byte array
        public Output(byte[] Data, int Offset, out int NewOffset)
        {
            // Get amount
            Amount = UnpackVarInt<ulong>(Data, Offset, out Offset);

            // Get output type
            Type = UnpackVarInt<byte>(Data, Offset, out Offset);

            // Get key
            PublicKey = Data.SubHexString(Offset, 32);

            // Adjust offset to account for key image length
            Offset += 32;

            // Set new offset
            NewOffset = Offset;
        }
    }
}
