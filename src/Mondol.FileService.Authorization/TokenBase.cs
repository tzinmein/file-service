// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
// 

using System;

namespace Mondol.FileService.Authorization
{
    public class TokenBase
    {
        /// <summary>
        /// Token expiration time
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// Indicates whether the token has expired
        /// </summary>
        public bool IsExpired => ExpireTime <= DateTime.Now;
    }
}
