// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
// 

using System;

namespace Mondol.FileService.Authorization.Codecs.Impls
{
    /// <summary>
    /// IFileTokenCodec兼容旧版本的实现
    /// </summary>
    public class FileTokenCompatibilityCodec : IFileTokenCodec
    {
        public FileTokenCompatibilityCodec()
        {
        }

        public byte CurrentVersion => throw new NotImplementedException();

        public FileToken Decode(string tokenStr)
        {
            throw new NotImplementedException();
        }

        public string Encode(FileToken token)
        {
            throw new NotImplementedException();
        }
    }
}
