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
            : this(clientId, clientSecret, accessToken, new HttpClientHandler())
        {
        }

        public FoursquareContext(string clientId, string clientSecret, string accessToken, HttpMessageHandler messageHandler)
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

            if (messageHandler == null)
            {
                throw new ArgumentNullException("messageHandler");
            }

            var versionAppender = new VersionAppenderHandler();
            var accessTokenAppender = new AccessTokenAppenderHandler(accessToken);
            var delegatingHandlers = new DelegatingHandler[] { versionAppender, accessTokenAppender };
            HttpMessageHandler innerHandler = HttpClientFactory.CreatePipeline(messageHandler, delegatingHandlers);
            _client = new HttpClient(innerHandler) { BaseAddress = new Uri(ServiceUri) };
            _clients = new ConcurrentDictionary<Type, object>();
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
