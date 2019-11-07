﻿//
// Copyright (c) 2019 Canti, The TurtleCoin Developers
//
// Please see the included LICENSE file for more information.

namespace Plenteum.Cryptography
{
    public sealed partial class PlenteumCrypto
    {
        // Minimum variation bytes required for certain hash functions (43 bytes)
        private const int MINIMUM_VARIATION_BYTES = 43 * 2;

        // Location of file
        public const string LIBRARY_LOCATION = "plenteum-crypto-shared";

        // Max string array size (in bytes)
        private const int MAXIMUM_ARRAY_SIZE = 32 * 32; // 32 hashes (arbitrary choice)
    }
}
