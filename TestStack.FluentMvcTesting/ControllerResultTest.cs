using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestStack.FluentMVCTesting
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

        public RouteValueDictionary ShouldRedirectToRoute(string route)
        {
            ValidateActionReturnType<RedirectToRouteResult>();
            var redirectResult = (RedirectToRouteResult)_actionResult;

            if (redirectResult.RouteName != route)
                throw new ActionResultAssertionException(string.Format("Expected redirect to route '{0}', but instead was given a redirect to route '{1}'.", route, redirectResult.RouteName));

            return redirectResult.RouteValues;
        }

        public RouteValueDictionary ShouldRedirectTo(Func<T, Func<ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public RouteValueDictionary ShouldRedirectTo(Func<T, Func<int, ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public RouteValueDictionary ShouldRedirectTo<T1>(Func<T, Func<T1, ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public RouteValueDictionary ShouldRedirectTo<T1, T2>(Func<T, Func<T1, T2, ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public RouteValueDictionary ShouldRedirectTo<T1, T2, T3>(Func<T, Func<T1, T2, T3, ActionResult>> actionRedirectedTo)
        {
            return ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public RouteValueDictionary ShouldRedirectTo(Expression<Action<T>> actionRedirectedTo)
        {
            var methodCall = (MethodCallExpression) actionRedirectedTo.Body;
            return ShouldRedirectTo(methodCall.Method);
        }

        public RouteValueDictionary ShouldRedirectTo(MethodInfo method, RouteValueDictionary expectedValues = null)
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

            var redirectResult = (RedirectToRouteResult)_actionResult;

            if (redirectResult.RouteValues["Controller"] == null)
                throw new ActionResultAssertionException(string.Format("Expected redirect to action '{0}' in '{1}' controller, but instead was given redirect to action '{2}' within the same controller.", actionName, controllerName, redirectResult.RouteValues["Action"]));

            if (redirectResult.RouteValues["Controller"].ToString() != controllerName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to controller '{0}', but instead was given a redirect to controller '{1}'.", controllerName, redirectResult.RouteValues["Controller"]));

            if (redirectResult.RouteValues["Action"].ToString() != actionName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to action '{0}', but instead was given a redirect to action '{1}'.", actionName, redirectResult.RouteValues["Action"]));

            return redirectResult.RouteValues;
        }

        #endregion

        #region View Results

        private ViewResultTest ShouldRenderViewResult<TViewResult>(string viewName) where TViewResult : ViewResultBase
        {
            ValidateActionReturnType<TViewResult>();

            var viewResult = (TViewResult)_actionResult;

            if (viewResult.ViewName != viewName && (viewName != _actionName || viewResult.ViewName != ""))
            {
                throw new ActionResultAssertionException(string.Format("Expected result view to be '{0}', but instead was given '{1}'.", viewName, viewResult.ViewName == "" ? _actionName : viewResult.ViewName));
            }

            return new ViewResultTest(viewResult, _controller);
        }

        public ViewResultTest ShouldRenderView(string viewName)
        {
            return ShouldRenderViewResult<ViewResult>(viewName);
        }

        public ViewResultTest ShouldRenderPartialView(string viewName)
        {
            return ShouldRenderViewResult<PartialViewResult>(viewName);
        }

        public ViewResultTest ShouldRenderDefaultView()
        {
            return ShouldRenderView(_actionName);
        }

        public ViewResultTest ShouldRenderDefaultPartialView()
        {
            return ShouldRenderPartialView(_actionName);
        }

        #endregion

        public FileStreamResult ShouldRenderFileStream()
        {
            ValidateActionReturnType<FileStreamResult>();
            return (FileStreamResult) _actionResult;
        }

        #region File Results

        public FileResult ShouldRenderAnyFile(string contentType = null)
        {
            ValidateActionReturnType<FileResult>();

            var fileResult = (FileResult)_actionResult;

            if (contentType != null && fileResult.ContentType != contentType)
            {
                throw new ActionResultAssertionException(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, fileResult.ContentType));
            }

            return fileResult;
        }

        public FileContentResult ShouldRenderFileContents(byte[] contents = null, string contentType = null)
        {
            ValidateActionReturnType<FileContentResult>();

            var fileResult = (FileContentResult) _actionResult;

            if (contentType != null && fileResult.ContentType != contentType)
            {
                throw new ActionResultAssertionException(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, fileResult.ContentType));
            }

            if (contents != null && !fileResult.FileContents.SequenceEqual(contents))
            {
                throw new ActionResultAssertionException(string.Format(
                    "Expected file contents to be equal to [{0}], but instead was given [{1}].",
                    string.Join(", ", contents),
                    string.Join(", ", fileResult.FileContents)));
            }

            return fileResult;
        }

        public FileContentResult ShouldRenderFileContents(string contents, string contentType = null, Encoding encoding = null)
        {
            ValidateActionReturnType<FileContentResult>();

            var fileResult = (FileContentResult)_actionResult;

            if (contentType != null && fileResult.ContentType != contentType)
            {
                throw new ActionResultAssertionException(
                    string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType,
                        fileResult.ContentType));
            }

            if (encoding == null)
                encoding = Encoding.UTF8;

            var reconstitutedText = encoding.GetString(fileResult.FileContents);
            if (contents != reconstitutedText)
            {
                throw new ActionResultAssertionException(string.Format("Expected file contents to be \"{0}\", but instead was \"{1}\".", contents, reconstitutedText));
            }

            return fileResult;
        }

        [Obsolete("Obsolete: Use ShouldRenderFileContents instead.")]
        public FileContentResult ShouldRenderFile(string contentType = null)
        {
            ValidateActionReturnType<FileContentResult>();

            var fileResult = (FileContentResult)_actionResult;

            if (contentType != null && fileResult.ContentType != contentType)
            {
                throw new ActionResultAssertionException(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, fileResult.ContentType));
            }

            return fileResult;
        }

        public FilePathResult ShouldRenderFilePath(string fileName = null, string contentType = null)
        {
            ValidateActionReturnType<FilePathResult>();

            var fileResult = (FilePathResult)_actionResult;

            if (contentType != null && fileResult.ContentType != contentType)
            {
                throw new ActionResultAssertionException(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, fileResult.ContentType));
            }

            if (fileName != null && fileName != fileResult.FileName)
            {
                throw new ActionResultAssertionException(string.Format("Expected file name to be '{0}', but instead was given '{1}'.", fileName, fileResult.FileName));
            }

            return fileResult;
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

        public void ShouldGiveHttpStatus(HttpStatusCode status)
        {
            ShouldGiveHttpStatus((int) status);
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