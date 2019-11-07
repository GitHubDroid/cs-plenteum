﻿//
// Copyright (c) 2018-2019 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Plenteum.Cryptography;
using static Plenteum.Utils;

namespace Plenteum.CryptoNote.Blockchain
{
    public sealed class Transaction
    {
        #region Properties and Fields

        public long Id { get; set; }
        // Transaction hash - returns a predefined hash or hashes current data
        private string _hash;
        public string Hash
        {
            get
            {
                // Check if hash is predefined
                if (!string.IsNullOrEmpty(_hash)) return _hash;

                // Set hash
                SetHash();

                // Return hash
                return _hash;
            }
            set
            {
                // Set pre-defined hash
                _hash = value;
            }
        }

        // Hashed Values
        public byte Version { get; set; }
        public ulong UnlockTime { get; set; }
        internal Input[] Inputs { get; set; }
        internal Output[] Outputs { get; set; }

        public string PublicKey { get; set; }
        public byte[] Extra { get; set; }
        public byte[] InputsOutputs
        {
            get
            {
                byte[] Buffer = new byte[0];
                // Add inputs
                Buffer = Buffer.AppendVarInt(Inputs.Length);
                foreach (var Input in Inputs)
                {
                    // Add serialized input bytes
                    Buffer = Buffer.AppendBytes(Input.Serialize());
                }

                // Add outputs
                Buffer = Buffer.AppendVarInt(Outputs.Length);
                foreach (var Output in Outputs)
                {
                    // Add serialized output bytes
                    Buffer = Buffer.AppendBytes(Output.Serialize());
                }
                return Buffer;
            }
            set
            {
                int Offset = 0;
                uint InputCount = UnpackVarInt<uint>(value, Offset, out Offset);
                List<Input> InputList = new List<Input>();
                for (uint i = 0; i < InputCount; i++)
                {
                    // Deserialize input
                    Input Input = new Input(value, Offset, out Offset);

                    // Add input amount
                    TotalFee += Input.Amount;

                    // Store input
                    InputList.Add(Input);
                }
                Inputs = InputList.ToArray();

                // Get outputs
                List<Output> OutputList = new List<Output>();
                uint OutputCount = UnpackVarInt<uint>(value, Offset, out Offset);
                for (uint i = 0; i < OutputCount; i++)
                {
                    // Deserialize output
                    Output Output = new Output(value, Offset, out Offset);

                    // Add output amount
                    TotalAmount += Output.Amount;

                    // Store output
                    OutputList.Add(Output);
                }
                Outputs = OutputList.ToArray();
            }
        }
        // Convenience
        public string BlockHash { get; set; }
        public string PaymentId { get; set; }
        public ulong Size { get; set; }
        public ulong TotalAmount { get; set; }
        public ulong TotalFee { get; set; }
        public byte Mixin { get; set; }

        // Verification
        public bool Verified { get; set; }
        public byte[] Signatures { get; set; }

        #endregion

        #region Methods

        internal void SetHash()
        {
            // Get hashing array
            var HashingArray = GetRawTransaction();

            // Store computed hash
            _hash = Crypto.CN_FastHash(HashingArray);
        }

        internal byte[] GetRawTransaction()
        {
            // Create a byte array buffer to work on
            byte[] Buffer = new byte[0];

            // Add version and unlock height
            Buffer = Buffer.AppendVarInt(Version);
            Buffer = Buffer.AppendVarInt(UnlockTime);

            // Add inputs
            Buffer = Buffer.AppendVarInt(Inputs.Length);
            foreach (var Input in Inputs)
            {
                // Add serialized input bytes
                Buffer = Buffer.AppendBytes(Input.Serialize());
            }

            // Add outputs
            Buffer = Buffer.AppendVarInt(Outputs.Length);
            foreach (var Output in Outputs)
            {
                // Add serialized output bytes
                Buffer = Buffer.AppendBytes(Output.Serialize());
            }

            // Add extra
            Buffer = Buffer.AppendVarInt(Extra.Length);
            Buffer = Buffer.AppendBytes(Extra);

            // Serialization complete
            return Buffer;
        }

        #endregion

        #region Constructors

        // Initializes an empty transaction
        public Transaction()
        {
            Version = Constants.TRANSACTION_VERSION;
            Hash = Constants.NULL_HASH;
            PaymentId = Constants.NULL_HASH;
            Size = 0;
            TotalAmount = 0;
            TotalFee = 0;
            Mixin = 0;
            UnlockTime = 0;
            Verified = false;

            Inputs = new Input[0];
            Outputs = new Output[0];
            Signatures = new byte[0];

            Extra = new byte[0];
        }

        // Deserializes a transaction from a hex string
        internal Transaction(string Hex)
        {
            /*try
            {*/
            // Convert to a byte array to work with
            byte[] Data = HexStringToByteArray(Hex);

            // Get hash and size
            Hash = Crypto.CN_FastHash(Hex);
            Size = (ulong)Data.LongLength;

            //BlockHash = blockHash;

            // Get version and unlock time (height?)
            Version = UnpackVarInt<byte>(Data, 0, out int Offset);
            UnlockTime = UnpackVarInt<uint>(Data, Offset, out Offset);

            // Get inputs
            uint InputCount = UnpackVarInt<uint>(Data, Offset, out Offset);
            List<Input> InputList = new List<Input>();
            for (uint i = 0; i < InputCount; i++)
            {
                // Deserialize input
                Input Input = new Input(Data, Offset, out Offset);

                // Add input amount
                TotalFee += Input.Amount;

                // Store input
                InputList.Add(Input);
            }
            Inputs = InputList.ToArray();

            // Get outputs
            List<Output> OutputList = new List<Output>();
            uint OutputCount = UnpackVarInt<uint>(Data, Offset, out Offset);
            for (uint i = 0; i < OutputCount; i++)
            {
                // Deserialize output
                Output Output = new Output(Data, Offset, out Offset);

                // Add output amount
                TotalAmount += Output.Amount;

                // Store output
                OutputList.Add(Output);
            }
            Outputs = OutputList.ToArray();

            // Get extra size
            int ExtraSize = UnpackVarInt<int>(Data, Offset, out Offset);

            // Get public key and extra data
            // TODO - better extra processing (PID etc.)
            Data = Data.SubBytes(Offset, ExtraSize);
            PublicKey = ByteArrayToHexString(Data.SubBytes(1, 32));
            Extra = Data;

            // TODO - Fill in the following:
            PaymentId = Constants.NULL_HASH;

            Mixin = 0;
            Verified = false;
            Signatures = new byte[0];
            /*}
            catch { }*/
        }

        #endregion
    }
}
