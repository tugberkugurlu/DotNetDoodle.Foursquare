using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDoodle.Foursquare.Clients
{
    public class ListClient : IListClient
    {
        private readonly HttpClient _client;

        public ListClient(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            _client = client;
        }

        /// <summary>
        /// Retrives the lists for the acting user by passing 'self' for the USER_ID.
        /// </summary>
        public Task Get()
        {
            return Get("self");
        }

        public async Task Get(string userId)
        {
            string uri = string.Format("v2/users/{0}/lists", userId);
            HttpResponseMessage response = await _client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                FoursquareContent<FoursquareListResponseResource> content = await response.Content.ReadAsAsync<FoursquareContent<FoursquareListResponseResource>>().ConfigureAwait(false);
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
        }
    }

    /// <summary>
    /// https://developer.foursquare.com/docs/responses/list
    /// </summary>
    public class ListResource
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserResource User { get; set; }

        public bool Public { get; set; }
        public bool Following { get; set; }
        public bool Editable { get; set; }
        public bool Collaborative { get; set; }

        public FollowersSummaryResource Followers { get; set; }
        public int Collaborators { get; set; }
        public int VenueCount { get; set; }
        public int VisitedCount { get; set; }

        public string Url { get; set; }
        public string CanonicalUrl { get; set; }
    }

    public class FollowersSummaryResource
    {
        public int Count { get; set; }
    }

    public class UserResource
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Relationship { get; set; }
        public PhotoSummaryResource Photo { get; set; }
    }

    public class PhotoSummaryResource
    {
        public string Prefix { get; set; }
        public string Suffix { get; set; }
    }

    public class FoursquareContent<TResponse>
    {
        public FoursquareMetaResource Meta { get; set; }
        public IEnumerable<FoursquareNotificationResource> Notifications { get; set; }
        public TResponse Response { get; set; }

        public class FoursquareMetaResource
        {
            public int Code { get; set; }
        }

        public class FoursquareNotificationResource
        {
            public string Type { get; set; }
            public FoursquareItemResosurce Item { get; set; }


            public class FoursquareItemResosurce
            {
                public string UnreadCount { get; set; }
            }
        }
    }

    public class FoursquareListResponseResource
    {
        public ListsResource Lists { get; set; }

        public class ListsResource
        {
            public int Count { get; set; }
            public IEnumerable<GroupResource> Groups { get; set; }

            public class GroupResource
            {
                public string Type { get; set; }
                public string Name { get; set; }
                public int Count { get; set; }
                public IEnumerable<ListResource> Items { get; set; }
            }
        }
    }
}
