using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    public class PipelineActionExecutor : IActionExecutor
    {
        public ControllerResultTest<TController> Execute<TController, TAction>(TController controller, Expression<Func<TController, TAction>> actionCall) 
            where TController : Controller 
            where TAction : ActionResult
        {
            var routes = GetDefaultRoutes();
            var action = new MvcControllerAction(controller, MvcActionDescriptor.Create(actionCall), routes)
            {
                ValueProvider = new MvcActionValueProvider(actionCall.Body),
                HttpMethod = "Get"
            };
            var httpContext = action.CreateHttpContext();
            var result = action.Execute(httpContext);
            return new ControllerResultTest<TController>((TController)result.Controller, action.ActionDescriptor.ActionName, result.ActionResult);
        }

        public ControllerResultTest<TController> Execute<TController, TAction>(TController controller, Expression<Func<TController, Task<TAction>>> actionCall) where TController : Controller where TAction : ActionResult
        {
            var routes = GetDefaultRoutes();
            var action = new MvcControllerAction(controller, MvcActionDescriptor.Create(actionCall), routes)
            {
                ValueProvider = new MvcActionValueProvider(actionCall.Body),
                HttpMethod = "Get"
            };
            var httpContext = action.CreateHttpContext();
            var result = action.Execute(httpContext);
            return new ControllerResultTest<TController>((TController)result.Controller, action.ActionDescriptor.ActionName, result.ActionResult);
        }

        private static RouteCollection GetDefaultRoutes()
        {
            var routes = new RouteCollection();
            if (RouteTable.Routes.Count == 0)
            {
                routes.MapRoute(
                    "Default",
                    "{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                    );
            }
            else
            {
                foreach (var route in RouteTable.Routes)
                    routes.Add(route);
            }

            return routes;
        }

    }
}