using System.Linq;
using System.Web;

namespace Xania.AspNet.Simulator
{
    public static class RouterExtensions
    {
        public static RouterAction Action(this Router router, string url)
        {
            return new RouterAction(router) { UriPath = url };
        }

        public static RouterAction ParseAction(this Router router, string rawHttpRequest)
        {
            var lines = rawHttpRequest.Split('\n');
            var first = lines.First();

            var parts = first.Split(' ');
            return new RouterAction(router)
            {
                HttpMethod = parts[0],
                UriPath = parts[1]
            };
        }
    }
}
