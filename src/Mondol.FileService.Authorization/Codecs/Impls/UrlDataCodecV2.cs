// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
// 

using System;
using System.Text.RegularExpressions;

namespace Mondol.FileService.Authorization.Codecs.Impls
{
    /// <summary>
    /// Data codec carried by URL
    /// </summary>
    public class UrlDataCodecV2 : IUrlDataCodec
    {
        /// <summary>
        /// Container version, increasing from 1-9, A-Z, a-z (ASCII code is getting bigger and bigger)
        /// </summary>
        public const char CurrentVersion = '2';

        public string Encode(byte[] data)
        {
            var encStr = System.Convert.ToBase64String(data);
            return CurrentVersion + encStr.Replace('+', '-').Replace('/', '~').TrimEnd('=');
        }

        public byte[] Decode(string encedStr)
        {
            if (encedStr == null || encedStr.Length < 2)
            {
                throw new ArgumentException(nameof(encedStr));
            }
            if (encedStr[0] != CurrentVersion)
            {
                throw new NotSupportedException("bad container version");
            }

            //Remove the version number and replace from the second character
            encedStr = encedStr[1..];
            encedStr = encedStr.Replace('-', '+').Replace('~', '/');
            if (encedStr.Length % 4 > 0)
            {
                encedStr = encedStr.PadRight(encedStr.Length + (encedStr.Length % 4), '=');
            }

            return System.Convert.FromBase64String(encedStr);
        }
    }
}
