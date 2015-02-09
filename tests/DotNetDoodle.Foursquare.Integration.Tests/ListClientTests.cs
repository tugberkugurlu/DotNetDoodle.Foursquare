using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DotNetDoodle.Foursquare.Integration.Tests
{
    public class ListClientTests
    {
        [Test]
        public async Task Get_Should_Receive_200_Response()
        {
            var clienId = Environment.GetEnvironmentVariable("DOTNETDOODLE_FOURSQUARE_CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("DOTNETDOODLE_FOURSQUARE_CLIENT_SECRET");
            var accessToken = Environment.GetEnvironmentVariable("DOTNETDOODLE_FOURSQUARE_ACCESS_TOKEN");

            FoursquareContext context = new FoursquareContext(clienId, clientSecret, accessToken);
            await context.Lists.Get();
        }
    }
}