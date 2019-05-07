//
// Copyright (c) 2018 The TurtlePay™ Developers, Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace CantiLib.Database
{
    // DEBUG / INCOMPLETE
    public static class Commands
    {
        /* BLOCKS */
        // Creating the blocks table
        const string CREATE_BLOCKS_TABLE =
            @"CREATE TABLE IF NOT EXISTS blocks (height INT, hash VARCHAR(64), prevHash VARCHAR(64))";

        // Inserting a block into blocks table
        const string INSERT_BLOCK =
            @"INSERT INTO `blocks` (
            `height`, `hash`, `prevHash`, `baseReward`, `difficulty`, `majorVersion`, 
            `minorVersion`, `nonce`, `size`, `timestamp`, `alreadyGeneratedCoins`, 
            `alreadyGeneratedTransactions`, `reward`, `sizeMedian`, `totalFeeAmount`, 
            `transactionsCumulativeSize`',
            ) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

        /* TRANSACTIONS */

        /* MISCELLANEOUS */

    }
}
