using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace TerseControllerTesting
{
    public class ControllerResultTest<T> where T : Controller
    {
        #region Private properties and methods
        private readonly T _controller;
        private readonly string _actionName;
        private readonly ActionResult _actionResult;

        private void ValidateActionReturnType<TActionResult>() where TActionResult : ActionResult
        {
            var castedActionResult = _actionResult as TActionResult;

            if (_actionResult == null)
                throw new ActionResultAssertionException(string.Format("Received null action result when expecting {0}.", typeof(TActionResult).Name));

            if (castedActionResult == null)
                throw new ActionResultAssertionException(
                    string.Format("Expected action result to be a {0}, but instead received a {1}.",
                                  typeof(TActionResult).Name, _actionResult.GetType().Name
                        )
                    );
        }
        #endregion

        #region Ctor

        public ControllerResultTest(T controller, string actionName, ActionResult actionResult)
        {
            _controller = controller;
            _actionName = actionName;
            _actionResult = actionResult;
        }

        #endregion

        #region Empty Result

        public void ShouldReturnEmptyResult()
        {
            ValidateActionReturnType<EmptyResult>();
        }

        #endregion

        #region Redirects

        public void ShouldRedirectTo(string url)
        {
            ValidateActionReturnType<RedirectResult>();
            var redirectResult = (RedirectResult)_actionResult;

            if (redirectResult.Url != url)
                throw new ActionResultAssertionException(string.Format("Expected redirect to URL '{0}', but instead was given a redirect to URL '{1}'.", url, redirectResult.Url));
        }

        public void ShouldRedirectToRoute(string route)
        {
            ValidateActionReturnType<RedirectToRouteResult>();
            var redirectResult = (RedirectToRouteResult)_actionResult;

            if (redirectResult.RouteName != route)
                throw new ActionResultAssertionException(string.Format("Expected redirect to route '{0}', but instead was given a redirect to route '{1}'.", route, redirectResult.RouteName));
        }

        public void ShouldRedirectTo(Func<T, Func<ActionResult>> actionRedirectedTo)
        {
            ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public void ShouldRedirectTo(Func<T, Func<int, ActionResult>> actionRedirectedTo)
        {
            ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public void ShouldRedirectTo<T1>(Func<T, Func<T1, ActionResult>> actionRedirectedTo)
        {
            ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public void ShouldRedirectTo<T1, T2>(Func<T, Func<T1, T2, ActionResult>> actionRedirectedTo)
        {
            ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public void ShouldRedirectTo<T1, T2, T3>(Func<T, Func<T1, T2, T3, ActionResult>> actionRedirectedTo)
        {
            ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public void ShouldRedirectTo(Expression<Action<T>> actionRedirectedTo)
        {
            var methodCall = (MethodCallExpression) actionRedirectedTo.Body;
            ShouldRedirectTo(methodCall.Method);
        }

        public void ShouldRedirectTo(MethodInfo method, RouteValueDictionary expectedValues = null)
        {
            ValidateActionReturnType<RedirectToRouteResult>();

            var controllerName = new Regex(@"Controller$").Replace(typeof(T).Name, "");
            var actionName = method.Name;
            var redirectResult = (RedirectToRouteResult)_actionResult;

            if (redirectResult.RouteValues.ContainsKey("Controller") && redirectResult.RouteValues["Controller"].ToString() != controllerName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to controller '{0}', but instead was given a redirect to controller '{1}'.", controllerName, redirectResult.RouteValues["Controller"]));

            if (!redirectResult.RouteValues.ContainsKey("Action"))
                throw new ActionResultAssertionException(string.Format("Expected redirect to action '{0}', but instead was given a redirect without an action.", actionName));

            if (redirectResult.RouteValues["Action"].ToString() != actionName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to action '{0}', but instead was given a redirect to action '{1}'.", actionName, redirectResult.RouteValues["Action"]));

            if (expectedValues == null)
                return;

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
        }

        public void ShouldRedirectTo<TController>(MethodInfo methodInfo) where TController : Controller
        {
            ValidateActionReturnType<RedirectToRouteResult>();

            var controllerName = new Regex(@"Controller$").Replace(typeof(TController).Name, "");
            var actionName = methodInfo.Name;

            var redirectResult = (RedirectToRouteResult)_actionResult;

            if (redirectResult.RouteValues["Controller"].ToString() != controllerName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to controller '{0}', but instead was given a redirect to controller '{1}'.", controllerName, redirectResult.RouteValues["Controller"]));

            if (redirectResult.RouteValues["Action"].ToString() != actionName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to action '{0}', but instead was given a redirect to action '{1}'.", actionName, redirectResult.RouteValues["Action"]));
        }

        #endregion

        #region View Results

        public ViewResultTest ShouldRenderView(string viewName)
        {
            ValidateActionReturnType<ViewResult>();

            var viewResult = (ViewResult)_actionResult;

            if (viewResult.ViewName != viewName && (viewName != _actionName || viewResult.ViewName != ""))
            {
                throw new ActionResultAssertionException(string.Format("Expected result view to be '{0}', but instead was given '{1}'.", viewName, viewResult.ViewName == "" ? _actionName : viewResult.ViewName));
            }

            return new ViewResultTest(viewResult, _controller);
        }

        public ViewResultTest ShouldRenderPartialView(string viewName)
        {
            ValidateActionReturnType<PartialViewResult>();

            var viewResult = (PartialViewResult)_actionResult;

            if (viewResult.ViewName != viewName && (viewName != _actionName || viewResult.ViewName != ""))
            {
                throw new ActionResultAssertionException(string.Format("Expected result view to be '{0}', but instead was given '{1}'.", viewName, viewResult.ViewName == "" ? _actionName : viewResult.ViewName));
            }

            return new ViewResultTest(viewResult, _controller);
        }

        public void ShouldRenderFile(string contentType = null)
        {
            ValidateActionReturnType<FileContentResult>();

            var fileResult = (FileContentResult)_actionResult;

            if (contentType != null && fileResult.ContentType != contentType)
            {
                throw new ActionResultAssertionException(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, fileResult.ContentType));
            }
        }

        public ViewResultTest ShouldRenderDefaultView()
        {
            ValidateActionReturnType<ViewResult>();

            var viewResult = (ViewResult)_actionResult;

            if (viewResult.ViewName != "" && viewResult.ViewName != _actionName)
            {
                throw new ActionResultAssertionException(string.Format("Expected result view to be '{0}', but instead was given '{1}'.", _actionName, viewResult.ViewName));
            }

            return new ViewResultTest(viewResult, _controller);
        }

        public ViewResultTest ShouldRenderDefaultPartialView()
        {
            ValidateActionReturnType<PartialViewResult>();

            var viewResult = (PartialViewResult)_actionResult;

            if (viewResult.ViewName != "" && viewResult.ViewName != _actionName)
            {
                throw new ActionResultAssertionException(string.Format("Expected result view to be '{0}', but instead was given '{1}'.", _actionName, viewResult.ViewName));
            }

            return new ViewResultTest(viewResult, _controller);
        }

        #endregion

        #region Http Status

        public void ShouldGiveHttpStatus()
        {
            ValidateActionReturnType<HttpStatusCodeResult>();
        }

        public void ShouldGiveHttpStatus(int status)
        {
            ValidateActionReturnType<HttpStatusCodeResult>();

            var statusCodeResult = (HttpStatusCodeResult)_actionResult;

            if (statusCodeResult.StatusCode != status)
                throw new ActionResultAssertionException(string.Format("Expected HTTP status code to be '{0}', but instead received a '{1}'.", status, statusCodeResult.StatusCode));
        }

        #endregion

        #region JSON
        public void ShouldReturnJson()
        {
            ValidateActionReturnType<JsonResult>();
        }

        public void ShouldReturnJson(Action<dynamic> assertion)
        {
            ValidateActionReturnType<JsonResult>();
            var jsonResult = (JsonResult)_actionResult;
            assertion(jsonResult.Data);
        }
        #endregion
    }
}