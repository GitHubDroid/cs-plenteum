//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Data;
using System;
using System.Linq;

namespace Canti.Blockchain.Crypto
{
    public class TreeHash
    {
        static byte[] hash_process(byte[] state, byte[] buf)
        {
            return Keccak.Keccak.keccak1600(buf, state);
        }
        
        public static byte[] cn_fast_hash(byte[] data, int length, byte[] hash)
        {
            byte[] state = new byte[length];
            hash = hash_process(state, Encoding.AppendToByteArray(data, hash));
            Array.Copy(hash, state, length);
            return hash;
        }

        // Hashes a set of strings into a single hash
        public static string Hash(char[] hashes, char[] root_hash)
        {
            // If input array only has one member
            if (hashes.Length == 1)
            {
                hashes = new char[32];
                Array.Copy(root_hash, hashes, 32);
            }

            // Input array has 2 members
            else if (hashes.Length == 2)
            {
                hashes = cn_fast_hash(hashes.Select(c => (byte)c).ToArray(), 2 * 32, root_hash.Select(c => (byte)c).ToArray()).Select(c => (char)c).ToArray();
            }

            // Input array as more than 2 members
            else
            {
                int i, j;
                int cnt = hashes.Length - 1;
                for (i = 1; i < 8 * sizeof(int); i <<= 1)
                    cnt |= cnt >> 1;
                cnt &= ~(cnt >> 1);
                char[] ints = new char[cnt * 32];
                Array.Copy(hashes, ints, (2 * cnt - hashes.Length) * 32);
                for (i = 2 * cnt - hashes.Length, j = 2 * cnt - hashes.Length; j < cnt; i += 2, ++j)
                    hashes = cn_fast_hash(new byte[] { (byte)hashes[i] }, 2 * 32, new byte[] { (byte)ints[j] }).Select(c => (char)c).ToArray();
                while (cnt > 2)
                {
                    cnt >>= 1;
                    for (i = 0, j = 0; j < cnt; i += 2, ++j)
                        hashes = cn_fast_hash(new byte[] { (byte)ints[i] }, 2 * 32, new byte[] { (byte)ints[j] }).Select(c => (char)c).ToArray();
                }
                hashes = cn_fast_hash(new byte[] { (byte)ints[0] }, 2 * 32, root_hash.Select(c => (byte)c).ToArray()).Select(c => (char)c).ToArray();
            }

            return Encoding.StringToHexString(new string(hashes));
        }
        public static string Hash(string[] hashes, string root_hash)
        {
            return Hash(ReinterpretCast<string[], char[]>(hashes), root_hash.ToCharArray());
        }

        static unsafe TDest ReinterpretCast<TSource, TDest>(TSource source)
        {
            var sourceRef = __makeref(source);
            var dest = default(TDest);
            var destRef = __makeref(dest);
            *(IntPtr*)&destRef = *(IntPtr*)&sourceRef;
            return __refvalue(destRef, TDest);
        }
    }
}
