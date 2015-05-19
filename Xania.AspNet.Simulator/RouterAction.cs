using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Xania.AspNet.Simulator
{
    public class RouterAction : ControllerAction
    {
        private readonly Router _router;

        public RouterAction(Router router)
        {
            _router = router;
        }

        public override ControllerActionResult Execute(HttpContextBase httpContext)
        {
            var actionContext = GetActionContext(httpContext);
            var actionDescriptor = actionContext.ActionDescriptor;

            if (actionDescriptor == null)
                return null;

            return Execute(actionContext.ControllerContext, actionDescriptor);
        }

        public override ActionContext GetActionContext(HttpContextBase httpContext1)
        {
            var context = httpContext1 ?? AspNetUtility.GetContext(this);
            var routeData = _router.GetRouteData(context);

            if (routeData == null)
                return null;

            var controllerName = routeData.GetRequiredString("controller");
            var controller = _router.CreateController(controllerName);
            var controllerDescriptor = new ReflectedControllerDescriptor(controller.GetType());

            var valueProviders = new ValueProviderCollection();
            if (ValueProvider != null)
                valueProviders.Add(ValueProvider);
            valueProviders.Add(new RouterActionValueProvider(routeData, new CultureInfo("nl-NL")));
            controller.ValueProvider = valueProviders;

            var actionName = routeData.GetRequiredString("action");
            var httpContext = httpContext1 ?? AspNetUtility.GetContext(String.Format("/{0}/{1}", controllerName, actionName), HttpMethod, User ?? AspNetUtility.CreateAnonymousUser());

            var requestContext = new RequestContext(httpContext, routeData);

            controller.ControllerContext = new ControllerContext(requestContext, controller);
            if (controller is Controller)
            {
                (controller as Controller).Url = new UrlHelper(requestContext, _router.Routes);
            }


            return new ActionContext
            {
                ControllerContext = controller.ControllerContext,
                ActionDescriptor = controllerDescriptor.FindAction(controller.ControllerContext, actionName)
            };
        }

        public override HttpContextBase CreateHttpContext()
        {
            return AspNetUtility.GetContext(this);
        }
    }

    public class RouterActionValueProvider : IValueProvider
    {
        private readonly RouteData _routeData;
        private readonly CultureInfo _culture;

        public RouterActionValueProvider(RouteData routeData, CultureInfo culture)
        {
            _routeData = routeData;
            _culture = culture;
        }

        public bool ContainsPrefix(string prefix)
        {
            return _routeData.Values.ContainsKey(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            var value = _routeData.Values[key];
            return new ValueProviderResult(value, value.ToString(), _culture);
        }
    }
}