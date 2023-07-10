// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
// 

using System;

namespace Mondol.FileService.Authorization.Codecs.Impls
{
    /// <summary>
    /// IUrlDataCodec is compatible with the implementation of new and old versions
    /// </summary>
    public class UrlDataCompatibilityCodec : IUrlDataCodec
    {
        private readonly IUrlDataCodec _codecV1 = new UrlDataCodecV1();
        private readonly IUrlDataCodec _codecV2 = new UrlDataCodecV2();

        public string Encode(byte[] data)
        {
            return _codecV2.Encode(data);
        }

        public byte[] Decode(string encedStr)
        {
            if (encedStr == null || encedStr.Length < 2)
            {
                throw new ArgumentException(nameof(encedStr));
            }
            if (encedStr[0] == UrlDataCodecV1.CurrentVersion)
            {
                return _codecV1.Decode(encedStr);
            }

            return _codecV2.Decode(encedStr);
        }
    }
}
