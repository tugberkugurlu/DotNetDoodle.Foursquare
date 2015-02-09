using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDoodle.Foursquare.Clients
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _client;

        public UserClient(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            _client = client;
        }
    }
}
