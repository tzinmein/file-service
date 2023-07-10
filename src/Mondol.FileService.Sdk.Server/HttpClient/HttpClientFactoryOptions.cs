using System;
using System.Net.Http;

namespace Mondol.FileService.Server
{
    public class HttpClientFactoryOptions
    {
        /// <summary>
        /// 设置CreateClient时的名字，默认空
        /// </summary>
        public string HttpClientName { get; set; } = string.Empty;
        public Func<string, IServiceProvider, HttpMessageHandler> OnCreateHandler { get; set; }
        public Func<string, IServiceProvider, HttpClient> OnCreateClient { get; set; }
    }
}
