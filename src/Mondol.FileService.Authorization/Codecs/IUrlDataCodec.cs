// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
// 

namespace Mondol.FileService.Authorization.Codecs
{
    /// <summary>
    /// Data codec carried by URL
    /// </summary>
    public interface IUrlDataCodec
    {
        string Encode(byte[] data);

        byte[] Decode(string encedStr);
    }
}
