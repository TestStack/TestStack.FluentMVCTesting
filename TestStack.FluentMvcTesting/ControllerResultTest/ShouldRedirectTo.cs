using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        public void ShouldRedirectTo(string url)
        {
            ValidateActionReturnType<RedirectResult>();
            var redirectResult = (RedirectResult)ActionResult;

            if (redirectResult.Url != url)
                throw new ActionResultAssertionException($"Expected redirect to URL '{url}', but instead was given a redirect to URL '{redirectResult.Url}'.");
        }

        public RouteValueDictionary ShouldRedirectToRoute(string route)
        {
            ValidateActionReturnType<RedirectToRouteResult>();
            var redirectResult = (RedirectToRouteResult)ActionResult;

            if (redirectResult.RouteName != route)
                throw new ActionResultAssertionException($"Expected redirect to route '{route}', but instead was given a redirect to route '{redirectResult.RouteName}'.");

            return redirectResult.RouteValues;
        }

        public RouteValueDictionary ShouldRedirectTo(Func<T, Func<ActionResult>> actionRedirectedTo) => 
            ShouldRedirectTo(actionRedirectedTo(Controller).Method); 

        public RouteValueDictionary ShouldRedirectTo(Func<T, Func<int, ActionResult>> actionRedirectedTo) => 
            ShouldRedirectTo(actionRedirectedTo(Controller).Method);

        public RouteValueDictionary ShouldRedirectTo<T1>(Func<T, Func<T1, ActionResult>> actionRedirectedTo) => 
            ShouldRedirectTo(actionRedirectedTo(Controller).Method);

        public RouteValueDictionary ShouldRedirectTo<T1, T2>(Func<T, Func<T1, T2, ActionResult>> actionRedirectedTo) => 
            ShouldRedirectTo(actionRedirectedTo(Controller).Method);

        public RouteValueDictionary ShouldRedirectTo<T1, T2, T3>(Func<T, Func<T1, T2, T3, ActionResult>> actionRedirectedTo) => 
            ShouldRedirectTo(actionRedirectedTo(Controller).Method);

        public RouteValueDictionary ShouldRedirectTo(Expression<Action<T>> actionRedirectedTo)
        {
            var methodCall = (MethodCallExpression)actionRedirectedTo.Body;
            return ShouldRedirectTo(methodCall.Method);
        }

        public RouteValueDictionary ShouldRedirectTo(MethodInfo method, RouteValueDictionary expectedValues = null)
        {
            ValidateActionReturnType<RedirectToRouteResult>();

            var controllerName = new Regex(@"Controller$").Replace(typeof(T).Name, "");
            var actionName = method.Name;
            var redirectResult = (RedirectToRouteResult)ActionResult;

            if (redirectResult.RouteValues.ContainsKey("Controller") && redirectResult.RouteValues["Controller"].ToString() != controllerName)
                throw new ActionResultAssertionException($"Expected redirect to controller '{controllerName}', but instead was given a redirect to controller '{redirectResult.RouteValues["Controller"]}'.");

            if (!redirectResult.RouteValues.ContainsKey("Action"))
                throw new ActionResultAssertionException($"Expected redirect to action '{actionName}', but instead was given a redirect without an action.");

            if (redirectResult.RouteValues["Action"].ToString() != actionName)
                throw new ActionResultAssertionException($"Expected redirect to action '{actionName}', but instead was given a redirect to action '{redirectResult.RouteValues["Action"]}'.");

            if (expectedValues == null)
                return redirectResult.RouteValues;

            foreach (var expectedRouteValue in expectedValues)
            {
                object actualValue;
                if (!redirectResult.RouteValues.TryGetValue(expectedRouteValue.Key, out actualValue))
                {
                    throw new ActionResultAssertionException($"Expected redirect to have parameter '{expectedRouteValue.Key}', but it did not.");
                }
                if (actualValue.ToString() != expectedRouteValue.Value.ToString())
                {
                    throw new ActionResultAssertionException($"Expected parameter '{expectedRouteValue.Key}' to have value '{expectedRouteValue.Value}', but instead was given value '{actualValue}'.");
                }
            }

            return redirectResult.RouteValues;
        }

        public RouteValueDictionary ShouldRedirectTo<TController>(Expression<Action<TController>> actionRedirectedTo) where TController : Controller
        {
            var methodCall = (MethodCallExpression)actionRedirectedTo.Body;
            return ShouldRedirectTo<TController>(methodCall.Method);
        }

        public RouteValueDictionary ShouldRedirectTo<TController>(MethodInfo methodInfo) where TController : Controller
        {
            ValidateActionReturnType<RedirectToRouteResult>();

            var controllerName = new Regex(@"Controller$").Replace(typeof(TController).Name, "");
            var actionName = methodInfo.Name;

            var redirectResult = (RedirectToRouteResult)ActionResult;

            if (redirectResult.RouteValues["Controller"] == null && typeof(TController) == typeof(T))
            {
                return redirectResult.RouteValues;
            }

            if (redirectResult.RouteValues["Controller"] == null)
                throw new ActionResultAssertionException($"Expected redirect to action '{actionName}' in '{controllerName}' controller, but instead was given redirect to action '{redirectResult.RouteValues["Action"]}' within the same controller.");

            if (redirectResult.RouteValues["Controller"].ToString() != controllerName)
                throw new ActionResultAssertionException($"Expected redirect to controller '{controllerName}', but instead was given a redirect to controller '{redirectResult.RouteValues["Controller"]}'.");

            if (redirectResult.RouteValues["Action"].ToString() != actionName)
                throw new ActionResultAssertionException($"Expected redirect to action '{actionName}', but instead was given a redirect to action '{redirectResult.RouteValues["Action"]}'.");

            return redirectResult.RouteValues;
        }
    }
}