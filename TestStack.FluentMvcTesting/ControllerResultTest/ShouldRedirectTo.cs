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
                throw new ActionResultAssertionException(string.Format("Expected redirect to URL '{0}', but instead was given a redirect to URL '{1}'.", url, redirectResult.Url));
        }

        public RouteValueDictionary ShouldRedirectToRoute(string route)
        {
            ValidateActionReturnType<RedirectToRouteResult>();
            var redirectResult = (RedirectToRouteResult)ActionResult;

            if (redirectResult.RouteName != route)
                throw new ActionResultAssertionException(string.Format("Expected redirect to route '{0}', but instead was given a redirect to route '{1}'.", route, redirectResult.RouteName));

            return redirectResult.RouteValues;
        }

        public RouteValueDictionary ShouldRedirectTo(Func<T, Func<ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(Controller).Method);
        }

        public RouteValueDictionary ShouldRedirectTo(Func<T, Func<int, ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(Controller).Method);
        }

        public RouteValueDictionary ShouldRedirectTo<T1>(Func<T, Func<T1, ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(Controller).Method);
        }

        public RouteValueDictionary ShouldRedirectTo<T1, T2>(Func<T, Func<T1, T2, ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(Controller).Method);
        }

        public RouteValueDictionary ShouldRedirectTo<T1, T2, T3>(Func<T, Func<T1, T2, T3, ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(Controller).Method);
        }

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
                throw new ActionResultAssertionException(string.Format("Expected redirect to controller '{0}', but instead was given a redirect to controller '{1}'.", controllerName, redirectResult.RouteValues["Controller"]));

            if (!redirectResult.RouteValues.ContainsKey("Action"))
                throw new ActionResultAssertionException(string.Format("Expected redirect to action '{0}', but instead was given a redirect without an action.", actionName));

            if (redirectResult.RouteValues["Action"].ToString() != actionName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to action '{0}', but instead was given a redirect to action '{1}'.", actionName, redirectResult.RouteValues["Action"]));

            if (expectedValues == null)
                return redirectResult.RouteValues;

            foreach (var expectedRouteValue in expectedValues)
            {
                object actualValue;
                if (!redirectResult.RouteValues.TryGetValue(expectedRouteValue.Key, out actualValue))
                {
                    throw new ActionResultAssertionException(string.Format("Expected redirect to have parameter '{0}', but it did not.", expectedRouteValue.Key));
                }
                if (actualValue.ToString() != expectedRouteValue.Value.ToString())
                {
                    throw new ActionResultAssertionException(
                        string.Format("Expected parameter '{0}' to have value '{1}', but instead was given value '{2}'."
                                      , expectedRouteValue.Key
                                      , expectedRouteValue.Value
                                      , actualValue
                            ));
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

            if (redirectResult.RouteValues["Controller"] == null)
            //throw new ActionResultAssertionException(string.Format("Expected redirect to action '{0}' in '{1}' controller, but instead was given redirect to action '{2}' within the same controller.", actionName, controllerName, redirectResult.RouteValues["Action"]));
            {
                return redirectResult.RouteValues;
            }

            if (redirectResult.RouteValues["Controller"].ToString() != controllerName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to controller '{0}', but instead was given a redirect to controller '{1}'.", controllerName, redirectResult.RouteValues["Controller"]));

            if (redirectResult.RouteValues["Action"].ToString() != actionName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to action '{0}', but instead was given a redirect to action '{1}'.", actionName, redirectResult.RouteValues["Action"]));

            return redirectResult.RouteValues;
        }
    }
}