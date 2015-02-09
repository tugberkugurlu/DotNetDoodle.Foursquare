using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetDoodle.Foursquare.Infrastructure
{
    public class AccessTokenAppenderHandler : DelegatingHandler
    {
        private const string AccessTokenParameterName = "oauth_token";
        private readonly string _accessToken;

        public AccessTokenAppenderHandler(string accessToken)
        {
            if (accessToken == null)
            {
                throw new ArgumentNullException("accessToken");
            }

            _accessToken = accessToken;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.RequestUri = string.IsNullOrWhiteSpace(request.RequestUri.Query)
                ? new Uri(string.Concat(request.RequestUri.ToString(), "?", AccessTokenParameterName, "=", _accessToken))
                : new Uri(string.Concat(request.RequestUri.ToString(), "&", AccessTokenParameterName, "=", _accessToken));

            return base.SendAsync(request, cancellationToken);
        }
    }
}
