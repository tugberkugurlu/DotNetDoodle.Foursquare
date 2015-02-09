using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDoodle.Foursquare.Clients
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// Refer to <see cref="http://developer.foursquare.com/docs/lists/lists" /> for more information.
    /// </remarks>
    public interface IListClient
    {
        Task Get();
        Task Get(string userId);
    }
}
