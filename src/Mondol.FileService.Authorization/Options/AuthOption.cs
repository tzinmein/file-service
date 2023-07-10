// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
// 

using System.Text;

namespace Mondol.FileService.Authorization.Options
{
    public class AuthOption
    {
        /// <summary>
        /// Communication key with FileService
        /// </summary>
        public string AppSecret { get; set; }

        public byte[] GetAppSecretBytes() => Encoding.ASCII.GetBytes(AppSecret);
    }
}
