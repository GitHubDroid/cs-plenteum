//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Canti.Blockchain.Crypto;
using System;

namespace Canti.Blockchain
{
    [Serializable]
    public struct Output
    {
        public uint Amount { get; set; }
        public string Key { get; set; }
    }
}
