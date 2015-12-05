using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Xania.AspNet.Simulator
{
    public abstract class ControllerAction: IHttpRequest, IControllerAction
    {
        protected ControllerAction()
        {
            FilterProviders = new FilterProviderCollection(System.Web.Mvc.FilterProviders.Providers);
            Cookies = new Collection<HttpCookie>();
            Session = new Dictionary<string, object>();
            Files = new Dictionary<string, Stream>();
            Resolve = DependencyResolver.Current.GetService;
        }

        public FilterProviderCollection FilterProviders { get; private set; }

        public IPrincipal User { get; set; }

        public IValueProvider ValueProvider { get; set; }

        public ICollection<HttpCookie> Cookies { get; private set; }
        public IDictionary<string, object> Session { get; private set; }
        public IDictionary<string, Stream> Files { get; private set; }
        public IDictionary<string, string> Form { get; private set; }
        public Func<Type, object> Resolve { get; set; }

        public string HttpMethod { get; set; }

        public string UriPath { get; set; }

        public abstract ActionContext GetActionContext(HttpContextBase httpContext = null);

        public virtual ControllerActionResult Execute(HttpContextBase httpContext = null)
        {
            var actionContext = GetActionContext(httpContext ?? CreateHttpContext());
            return Execute(actionContext.ControllerContext, actionContext.ActionDescriptor);
        }

        protected virtual ControllerActionResult Execute(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var invoker = GetActionInvoker(controllerContext, actionDescriptor);
            return new ControllerActionResult
            {
                ControllerContext = controllerContext,
                ActionResult = invoker.InvokeAction()
            };
        }

        internal virtual SimulatorActionInvoker GetActionInvoker(ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var filters = FilterProviders.GetFilters(controllerContext, actionDescriptor).Select(BuildUp);

            return new SimulatorActionInvoker(controllerContext, actionDescriptor, filters);
        }

        protected virtual Filter BuildUp(Filter filter)
        {
            if (Resolve == null)
                return filter;

            foreach (PropertyDescriptor propertyDesc in TypeDescriptor.GetProperties(filter.Instance))
            {
                var typeNames = propertyDesc.Attributes.OfType<Attribute>().Select(e => e.GetType().ToString());
                if (typeNames.Contains("Microsoft.Practices.Unity.DependencyAttribute"))
                {
                    var service = Resolve(propertyDesc.PropertyType);
                    if (service != null)
                        propertyDesc.SetValue(filter.Instance, service);
                }
            }

            return filter;
        }

        public virtual ActionResult Authorize()
        {
            var actionContext = GetActionContext(CreateHttpContext());
            return Authorize(actionContext.ControllerContext, actionContext.ActionDescriptor);
        }

        protected virtual ActionResult Authorize(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return GetActionInvoker(controllerContext, actionDescriptor).AuthorizeAction();
        }

        public abstract HttpContextBase CreateHttpContext();
    }

    [Serializable]
    public class ControllerActionException : Exception
    {
        public ControllerActionException(string message)
            : base(message)
        {
        }
    }
}