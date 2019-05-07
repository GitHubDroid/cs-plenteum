//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System;

namespace Canti.Blockchain
{
    [Serializable]
    public struct Input
    {
        public uint Amount { get; set; }
        public uint[] OutputIndexes { get; set; }
        public string KeyImage { get; set; }
    }
}
