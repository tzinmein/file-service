// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
// 

using Mondol.Utils;
using System;
using System.Security.Cryptography;

namespace Mondol.FileService.Authorization.Codecs
{
    public interface ITokenCodec<TToken> where TToken : TokenBase
    {
        const int HashLen = 20;

        byte CurrentVersion { get; }

        string Encode(TToken token);

        TToken Decode(string tokenStr);

        static byte[] Datetime2Bytes(DateTime dt)
        {
            return NetBitConverter.GetBytes(DateTimeUtil.DateTimeToUnixTimestamp(dt));
        }

        static DateTime Bytes2DateTime(byte[] bytes, int startIndex)
        {
            var l = NetBitConverter.ToInt64(bytes, startIndex);
            return DateTimeUtil.UnixTimestampToDateTime(l);
        }

        static byte[] CalcHash(byte[] bytes)
        {
            return CalcHash(bytes, 0, bytes.Length);
        }

        static byte[] CalcHash(byte[] bytes, int offset, int count)
        {
            using var hashAlgo = SHA1.Create();
            return hashAlgo.ComputeHash(bytes, offset, count);
        }
    }

    public static class TokenCodecExtensions
    {
        public static bool TryDecode<TToken>(this ITokenCodec<TToken> codec, string tokenStr, out TToken token) where TToken : TokenBase
        {
            try
            {
                token = codec.Decode(tokenStr);
                return true;
            }
            catch
            {
                token = null;
                return false;
            }
        }
    }
}
