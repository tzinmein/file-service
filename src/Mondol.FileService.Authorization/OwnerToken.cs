// Copyright (c) Mondol. All rights reserved.
// 
// Author:  frank
// Email:   frank@mondol.info
// Created: 2017-01-23
// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
//

using System;

namespace Mondol.FileService.Authorization
{
    /// <summary>
    /// Access token containing the identity information of the file owner
    /// </summary>
    public class OwnerToken : TokenBase
    {
        /// <summary>
        /// OwnerType field of FileOwner table
        /// </summary>
        public int OwnerType { get; set; }

        /// <summary>
        /// Id field of FileOwner table
        /// </summary>
        public int OwnerId { get; set; }
    }
}
