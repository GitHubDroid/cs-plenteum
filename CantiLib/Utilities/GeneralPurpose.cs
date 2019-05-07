//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Canti.Utilities
{
    public static class GeneralUtilities
    {
        // Returns the current unix timestamp
        public static ulong GetTimestamp()
        {
            return (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /* Thanks to - https://stackoverflow.com/a/1262619

           Shuffles a list of type <T> into a randomized order */
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /* Thanks to - https://stackoverflow.com/a/24087164/8737306

           Splits a list of type <T> into a List of Lists of type <T>, with
           chunks of size chunkSize

           E.g. you can use this to split a list of bytes into lists of bytes
           where each list is 8 bytes long */
        public static List<List<T>> ChunkBy<T>(this List<T> source,
                                               int chunkSize) 
        {
            return source
                  .Select((x, i) => new { Index = i, Value = x })
                  .GroupBy(x => x.Index / chunkSize)
                  .Select(x => x.Select(v => v.Value).ToList())
                  .ToList();
        }

        /* Thanks to - https://stackoverflow.com/a/38596841/8737306

           Takes the last N elements from an IEnumerable/List */
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source,
                                                 int numElements)
        {
            return source.Skip(Math.Max(0, source.Count() - numElements));
        }

        /* Thanks to - https://stackoverflow.com/a/50552122/8737306
        
           Deconstructs a dictionary into a tuple as you go, which is helpful
           for nicer syntax when iterating over a dictionary, rather than
           having to call entry.Key, entry.Value */
        public static IEnumerable<(TKey, TValue)> Tuples<TKey, TValue>(
            this IDictionary<TKey, TValue> dict)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in dict)
            {
                yield return (kvp.Key, kvp.Value);
            }
        }

        public static bool IsHex(this string str)
        {
            return str.All(c => IsHex(c));
        }

        /* Must be 0..9, or A..F, or a..f */
        public static bool IsHex(char c)
        {
            return ((c >= '0' && c <= '9') ||
                    (c >= 'a' && c <= 'f') ||
                    (c >= 'A' && c <= 'F'));
        }

        /* Is the filename being used by a file or directory already */
        public static bool FilenameInUse(string filename)
        {
            return File.Exists(filename) || Directory.Exists(filename);
        }
    }
}
