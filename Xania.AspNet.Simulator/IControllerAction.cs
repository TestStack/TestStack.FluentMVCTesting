using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Xania.AspNet.Simulator
{
    public interface IControllerAction
    {
        string HttpMethod { get; set; }

        string UriPath { get; set; }

        IPrincipal User { get; set; }

        FilterProviderCollection FilterProviders { get; }

        IValueProvider ValueProvider { get; }

        ControllerActionResult Execute(HttpContextBase httpContext);
    }
}
