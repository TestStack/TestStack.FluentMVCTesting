using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Xania.AspNet.Simulator
{
    public class DirectControllerAction : ControllerAction
    {
        public DirectControllerAction(ControllerBase controller, ActionDescriptor actionDescriptor, RouteCollection routes)
        {
            Controller = controller;
            ActionDescriptor = actionDescriptor;
            Routes = routes;
        }

        public virtual RouteCollection Routes { get; private set; }

        public virtual ControllerBase Controller { get; private set; }

        public virtual ActionDescriptor ActionDescriptor { get; private set; }

        public override ActionContext GetActionContext(HttpContextBase httpContext = null)
        {
            InitializeController(httpContext);

            return new ActionContext
            {
                ControllerContext = Controller.ControllerContext,
                ActionDescriptor = ActionDescriptor
            };
        }

        protected virtual void InitializeController(HttpContextBase httpContext)
        {
            var controllerContext = CreateControllerContext(httpContext ?? CreateHttpContext(), Controller,
                ActionDescriptor);

            // Use empty value provider by default to prevent use of ASP.NET MVC default value providers
            // Its not the purpose of this simulator framework to validate the ASP.NET MVC default value 
            // providers. Either a value provider is not need in case model values are predefined or a 
            // custom implementation is provided.
            Controller.ValueProvider = ValueProvider ?? new ValueProviderCollection();
            Controller.ControllerContext = controllerContext;
            var controller = Controller as Controller;
            if (controller != null)
            {
                controller.Url = new UrlHelper(controllerContext.RequestContext, Routes);
            }
        }

        public override HttpContextBase CreateHttpContext()
        {
            return CreateHttpContext(this, ActionDescriptor);
        }

        public virtual ControllerContext CreateControllerContext(HttpContextBase httpContext, ControllerBase controller, ActionDescriptor actionDescriptor)
        {
            var requestContext = GetRequestContext(httpContext, actionDescriptor);
            var controllerContext = new ControllerContext(requestContext, controller);

            if (actionDescriptor.GetSelectors().Any(selector => !selector.Invoke(controllerContext)))
            {
                throw new ControllerActionException(String.Format("Http method '{0}' is not allowed", controllerContext.HttpContext.Request.HttpMethod));
            }

            return controllerContext;
        }

        public HttpContextBase CreateHttpContext(IControllerAction actionRequest, ActionDescriptor actionDescriptor)
        {
            var controllerDescriptor = actionDescriptor.ControllerDescriptor;
            var controllerName = controllerDescriptor.ControllerName;

            var user = actionRequest.User ?? AspNetUtility.CreateAnonymousUser();
            var httpContext =
                AspNetUtility.GetContext(String.Format("/{0}/{1}", controllerName, actionDescriptor.ActionName),
                    actionRequest.HttpMethod, user);
            return httpContext;
        }

        protected virtual RequestContext GetRequestContext(HttpContextBase httpContext, ActionDescriptor actionDescriptor)
        {
            var requestContext = AspNetUtility.CreateRequestContext(httpContext,
                actionDescriptor.ControllerDescriptor.ControllerName,
                actionDescriptor.ActionName);

            foreach (var cookie in Cookies)
                requestContext.HttpContext.Request.Cookies.Add(cookie);

            foreach (var kvp in Session)
                // ReSharper disable once PossibleNullReferenceException
                requestContext.HttpContext.Session[kvp.Key] = kvp.Value;
            return requestContext;
        }
    }
}