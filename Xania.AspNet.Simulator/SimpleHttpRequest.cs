using System.Collections.Generic;
using System.Security.Principal;

namespace Xania.AspNet.Simulator
{
    internal class SimpleHttpRequest : IHttpRequest
    {
        public string UriPath { get; set; }
        public string HttpMethod { get; set; }
        public IPrincipal User { get; set; }
    }
}