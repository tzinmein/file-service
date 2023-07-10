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
    internal class UrlDataCodecV1 : IUrlDataCodec
    {
        /// <summary>
        /// Container version, increasing from 1-9, A-Z, a-z (ASCII code is getting bigger and bigger)
        /// </summary>
        public const char CurrentVersion = '1';

        private readonly Regex _rexBase64Enc = new Regex(@"[\+/=]", RegexOptions.Singleline | RegexOptions.Compiled);
        private readonly Regex _rexBase64Dec = new Regex(@"[_~\-]", RegexOptions.Singleline | RegexOptions.Compiled);

        public string Encode(byte[] data)
        {
            var encStr = System.Convert.ToBase64String(data);
            return CurrentVersion + _rexBase64Enc.Replace(encStr, m =>
                 m.Value switch
                {
                    "+" => "_",
                    "/" => "~",
                    "=" => "-",
                    _ => throw new InvalidOperationException(),
                }
            );
        }

        public byte[] Decode(string encedStr)
        {
            if (encedStr?.Length < 2)
            {
                throw new ArgumentException(nameof(encedStr));
            }
            if (encedStr[0] != CurrentVersion)
            {
                throw new NotSupportedException("bad container version");
            }

            //Remove the version number and replace from the second character
            encedStr = _rexBase64Dec.Replace(encedStr, m =>
                 m.Value switch
                {
                    "_" => "+",
                    "~" => "/",
                    "-" => "=",
                    _ => throw new InvalidOperationException(),
                }
            );

            return System.Convert.FromBase64String(encedStr.Substring(1));
        }
    }
}
