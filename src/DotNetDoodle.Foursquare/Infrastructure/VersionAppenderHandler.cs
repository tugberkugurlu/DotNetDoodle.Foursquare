using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetDoodle.Foursquare.Infrastructure
{
    internal class VersionAppenderHandler : DelegatingHandler
    {
        private const string ApiVersion = "v=20150208";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.RequestUri = string.IsNullOrWhiteSpace(request.RequestUri.Query)
                ? new Uri(string.Concat(request.RequestUri.ToString(), "?", ApiVersion))
                : new Uri(string.Concat(request.RequestUri.ToString(), "&", ApiVersion));

            return base.SendAsync(request, cancellationToken);
        }
    }
}
