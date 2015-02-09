using System;
using System.Collections.Concurrent;
using System.Net.Http;
using DotNetDoodle.Foursquare.Clients;
using DotNetDoodle.Foursquare.Infrastructure;

namespace DotNetDoodle.Foursquare
{
    public class FoursquareContext : IDisposable
    {
        private const string ServiceUri = "https://api.foursquare.com";

        private bool _disposed;
        private readonly HttpClient _client;
        private readonly ConcurrentDictionary<Type, object> _clients;

        public FoursquareContext(string clientId, string clientSecret, string accessToken)
        {
            if (clientId == null)
            {
                throw new ArgumentNullException("clientId");
            }

            if (clientSecret == null)
            {
                throw new ArgumentNullException("clientSecret");
            }

            if (accessToken == null)
            {
                throw new ArgumentNullException("accessToken");
            }

            VersionAppenderHandler versionAppender = new VersionAppenderHandler();
            AccessTokenAppenderHandler accessTokenAppender = new AccessTokenAppenderHandler(accessToken);
            HttpMessageHandler innerHandler = HttpClientFactory.CreatePipeline(
                new HttpClientHandler(),
                new DelegatingHandler[] { versionAppender, accessTokenAppender });

            _clients = new ConcurrentDictionary<Type, object>();
            _client = new HttpClient(innerHandler) { BaseAddress = new Uri(ServiceUri) };
        }

        public IUserClient Users
        {
            get { return GetClient<IUserClient>(() => new UserClient(_client)); }
        }

        public IListClient Lists
        {
            get { return GetClient<IListClient>(() => new ListClient(_client)); }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_client != null)
                {
                    _client.Dispose();
                }

                _disposed = true;
            }
        }

        // privates

        private TClient GetClient<TClient>(Func<TClient> valueFactory)
        {
            return (TClient)_clients.GetOrAdd(typeof(TClient), k => valueFactory());
        }
    }
}
