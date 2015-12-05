using System;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    public class WebHelpers
    {
        internal static RequestContext CreateRequestContext(string actionName, string controllerName, string httpMethod, IPrincipal user)
        {
            var httpContext = GetContext(String.Format("/{0}/{1}", controllerName, actionName), httpMethod, user);
            return CreateRequestContext(httpContext, controllerName, actionName);
        }

        internal static RequestContext CreateRequestContext(HttpContextBase httpContext, string controllerName, string actionName)
        {
            var routeData = new RouteData { Values = { { "controller", controllerName }, { "action", actionName } } };
            return new RequestContext(httpContext, routeData);
        }

        internal static HttpContextBase GetContext(string url, string method, IPrincipal user)
        {
            return GetContext(new SimpleHttpRequest
            {
                UriPath = url,
                User = user,
                HttpMethod = method
            });
        }

        internal static HttpContextBase GetContext(SimpleHttpRequest httpRequest)
        {
            var worker = new ActionRequestWrapper(httpRequest);
            var httpContext = new HttpContext(worker)
            {
                User = httpRequest.User ?? CreateAnonymousUser(),
            };

            return new HttpContextSimulator(httpContext);
        }

        public static IPrincipal CreateAnonymousUser()
        {
            return new GenericPrincipal(new GenericIdentity(String.Empty), new string[] { });
        }

    }
}
