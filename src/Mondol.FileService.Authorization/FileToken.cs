// Copyright (c) Mondol. All rights reserved.
// 
// Author:  frank
// Email:   frank@mondol.info
// Created: 2016-11-17
// ---------------------------------------------
// Reviewed by alan.yu @ 2021-07-08
// 

using System;

namespace Mondol.FileService.Authorization
{
    /// <summary>
    /// File access token containing basic file information
    /// </summary>
    public class FileToken : TokenBase
    {
        /// <summary>
        /// File pseudo ID
        /// </summary>
        public uint PseudoId { get; set; }

        /// <summary>
        /// Id field of File table
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Id field of FileOwner table
        /// </summary>
        public int FileOwnerId { get; set; }

        /// <summary>
        /// Id field of MIME class
        /// </summary>
        public uint MimeId { get; set; }

        /// <summary>
        /// File creation date
        /// </summary>
        public DateTime FileCreateTime { get; set; }
    }
}