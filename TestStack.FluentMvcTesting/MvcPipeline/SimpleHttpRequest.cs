using System.Security.Principal;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    internal class SimpleHttpRequest
    {
        public string UriPath { get; set; }
        public string HttpMethod { get; set; }
        public IPrincipal User { get; set; }
    }
}