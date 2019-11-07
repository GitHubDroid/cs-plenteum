// Copyright (c) 2019 The TurtleCoin Developers
//
// Please see the included LICENSE file for more information.

namespace Plenteum.Cryptography
{
    // Interface for cryptography implementations
    public interface IHashProvider
    {
        byte[] Hash(byte[] input);
    }
}
