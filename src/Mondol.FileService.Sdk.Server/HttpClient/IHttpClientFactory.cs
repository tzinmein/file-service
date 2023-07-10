using System;
using System.Net.Http;

namespace Mondol.FileService
{
    /// <summary>
    /// HttpClient工厂
    /// </summary>
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(string name);
    }

    public static class HttpClientFactoryExtensions
    {
        /// <summary>
        /// Creates a new <see cref="T:System.Net.Http.HttpClient" /> using the default configuration.
        /// </summary>
        public static HttpClient CreateClient(this IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            
            return factory.CreateClient(null);
        }
    }
}
