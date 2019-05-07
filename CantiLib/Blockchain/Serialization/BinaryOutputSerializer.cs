//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Data;
using System;
using System.Linq;

namespace Canti.Blockchain
{
    public class BinaryOutputSerializer
    {
        // Serializes an integer to a varint byte array
        public static byte[] SerializeVarInt<T>(T Value) where T : IConvertible
        {
            // Create an output array
            byte[] Output = new byte[0];

            // Convert value to a ulong
            ulong Converted = Convert.ToUInt64(Value);

            // Loop until full value is written
            while (Converted >= 0x80)
            {
                Encoding.AppendToByteArray((byte)(Converted | 0x80), Output);
                Converted >>= 7;
            }
            Encoding.AppendToByteArray((byte)(Converted), Output);

            // Return output array
            return Output;
        }

        // Serializes an array to a byte array
        public static byte[] SerializeArrayAsBinary(Array Value)
        {
            // Verify array is valid
            if (Value == null) return new byte[0];
            else if (!Value.GetType().IsArray) return new byte[0];

            // Create an output array
            byte[] Output = new byte[0];

            // Loop through all array entries
            for (int i = 0; i < Value.Length; i++)
            {
                // Encode object
                byte[] Buffer = SerializeObjectAsBinary(Value.GetValue(i));

                // Append to output array
                Output = Encoding.AppendToByteArray(Buffer, Output);
            }

            // Return output array
            return Output;
        }

        // Serializes an object to a binary array
        public static byte[] SerializeObjectAsBinary(object Value)
        {
            // Create an output array
            byte[] Output = new byte[0];

            // Check if null
            if (Value == null) return Output;

            // Get object type
            Type ObjType = Value.GetType();

            // Object is an integer
            if (ObjType == typeof(byte) || ObjType == typeof(sbyte) ||
                ObjType == typeof(ushort) || ObjType == typeof(short) ||
                ObjType == typeof(uint) || ObjType == typeof(int) ||
                ObjType == typeof(ulong) || ObjType == typeof(long))
                    Output = SerializeVarInt(Convert.ToUInt64(Value));

            // Object is a string
            else if (ObjType == typeof(string))
            {
                Output = SerializeVarInt(((string)Value).Length);
                Output = Encoding.AppendToByteArray(Encoding.StringToByteArray((string)Value), Output);
            }

            // Object is an array
            else if (ObjType.IsArray)
            {
                Output = SerializeArrayAsBinary((Array)Value);
            }

            // Property is an object
            else
            {
                Output = Encoding.ObjectToByteArray(Value);
            }

            // Return output array
            Console.WriteLine("Output byte array: {0}", Encoding.ByteArrayToHexString(Output));
            return Output;
        }
    }
}
