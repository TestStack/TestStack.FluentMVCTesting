using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace TerseControllerTesting
{

    #region ControllerResultTest

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
                throw new ActionResultAssertionException(string.Format("Received null action result when expecting {0}", typeof(TActionResult).Name));

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

        public void ShouldRedirectTo(Func<T, Func<object, ActionResult>> actionRedirectedTo)
        {
            ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public void ShouldRedirectTo(Func<T, Func<object, object, ActionResult>> actionRedirectedTo)
        {
            ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public void ShouldRedirectTo(Func<T, Func<object, object, object, ActionResult>> actionRedirectedTo)
        {
            ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public void ShouldRedirectTo(Func<T, Func<ActionResult>> actionRedirectedTo)
        {
            ShouldRedirectTo(actionRedirectedTo(_controller).Method);
        }

        public void ShouldRedirectTo(MethodInfo method, RouteValueDictionary expectedValues = null)
        {
            ValidateActionReturnType<RedirectToRouteResult>();

            var controllerName = new Regex(@"Controller$").Replace(typeof(T).Name, "");
            var actionName = method.Name;
            var redirectResult = (RedirectToRouteResult)_actionResult;

            if (redirectResult.RouteValues.ContainsKey("Controller") && redirectResult.RouteValues["Controller"].ToString() != controllerName)
                throw new ActionResultAssertionException(string.Format("Expected redirect to controller '{0}', but instead was given a redirect to controller '{1}'.", controllerName, redirectResult.RouteValues["Controller"]));

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

    #endregion

    #region Controller Extensions
    public static class ControllerExtensions
    {

        public static T WithModelErrors<T>(this T controller) where T : Controller
        {
            controller.ModelState.AddModelError("Key", "Value");
            return controller;
        }

        public static ControllerResultTest<T> WithCallTo<T, TAction>(this T controller, Expression<Func<T, TAction>> actionCall)
            where T : Controller
            where TAction : ActionResult
        {
            var actionName = ((MethodCallExpression)actionCall.Body).Method.Name;

            var actionResult = actionCall.Compile().Invoke(controller);

            return new ControllerResultTest<T>(controller, actionName, actionResult);
        }

        public static ControllerResultTest<T> WithCallToChild<T, TAction>(this T controller, Expression<Func<T, TAction>> actionCall)
            where T : Controller
            where TAction : ActionResult
        {
            var action = ((MethodCallExpression)actionCall.Body).Method;

            if (!action.IsDefined(typeof(ChildActionOnlyAttribute), false))
                throw new InvalidControllerActionException(string.Format("Expected action {0} of controller {1} to be a child action, but it didn't have the ChildActionOnly attribute.", action.Name, controller));

            return controller.WithCallTo(actionCall);
        }
    }
    #endregion

    #region ViewResultTest

    public class ViewResultTest
    {
        private readonly ViewResultBase _viewResult;
        private readonly Controller _controller;

        public ViewResultTest(ViewResultBase viewResult, Controller controller)
        {
            _viewResult = viewResult;
            _controller = controller;
        }

        public ModelTest<TModel> WithModel<TModel>() where TModel : class
        {
            var castedModel = _viewResult.Model as TModel;
            if (castedModel == null)
                throw new ViewResultModelAssertionException(string.Format("Expected view model to be of type '{0}'. It is actually null.", typeof(TModel).Name));

            return new ModelTest<TModel>(_controller);
        }

        public ModelTest<TModel> WithModel<TModel>(TModel expectedModel) where TModel : class
        {
            var test = WithModel<TModel>();

            var model = _viewResult.Model as TModel;
            if (model != expectedModel)
                throw new ViewResultModelAssertionException("Expected view model to be passed in model, but in fact it was a different model.");

            return test;
        }

        public ModelTest<TModel> WithModel<TModel>(Func<TModel, bool> predicate) where TModel : class
        {
            var test = WithModel<TModel>();

            var model = _viewResult.Model as TModel;
            if (!predicate(model))
                throw new ViewResultModelAssertionException("Expected view model to pass a condition, but it failed.");

            return test;
        }

        public ModelTest<TModel> WithModel<TModel>(Action<TModel> assertions) where TModel : class
        {
            var test = WithModel<TModel>();

            var model = _viewResult.Model as TModel;
            assertions(model);

            return test;
        }
    }

    #endregion

    #region ModelTest

    public class ModelTest<TModel>
    {
        private readonly Controller _controller;

        public ModelTest(Controller controller)
        {
            _controller = controller;
        }

        public ModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError)
        {
            var member = ((MemberExpression)memberWithError.Body).Member.Name;
            if (!_controller.ModelState.ContainsKey(member) || _controller.ModelState[member].Errors.Count == 0)
                throw new ViewResultModelAssertionException(string.Format("Expected controller '{0}' to have a model error for member '{1}', but none found.", _controller, member));
            return new ModelErrorTest<TModel>(this, member, _controller.ModelState[member].Errors);
        }

        public ModelErrorTest<TModel> AndModelError(string errorKey)
        {
            if (!_controller.ModelState.ContainsKey(errorKey) || _controller.ModelState[errorKey].Errors.Count == 0)
                throw new ViewResultModelAssertionException(string.Format("Expected controller '{0}' to have a model error against key '{1}', but none found.", _controller, errorKey));
            return new ModelErrorTest<TModel>(this, errorKey, _controller.ModelState[errorKey].Errors);
        }

        public void AndNoModelErrors()
        {
            if (!_controller.ModelState.IsValid)
                throw new ViewResultModelAssertionException(string.Format("Expected controller '{0}' to have no model errors, but it had some.", _controller));
        }
    }

    #endregion

    #region ModelErrorTest

    public class ModelErrorTest<TModel>
    {
        private readonly ModelTest<TModel> _modelTest;
        private readonly string _errorKey;
        private readonly List<string> _errors;

        public ModelErrorTest(ModelTest<TModel> modelTest, string errorKey, IEnumerable<ModelError> errors)
        {
            _modelTest = modelTest;
            _errorKey = errorKey;
            _errors = errors.Select(e => e.ErrorMessage).ToList();
        }

        public ModelTest<TModel> ThatEquals(string errorMessage)
        {
            if (!_errors.Any(e => e == errorMessage))
            {
                throw new ModelErrorAssertionException(string.Format("Expected error message for key '{0}' to be '{1}', but instead found '{2}'.", _errorKey, errorMessage, string.Join(", ", _errors)));
            }
            return _modelTest;
        }

        public ModelTest<TModel> BeginningWith(string beginMessage)
        {
            if (!_errors.Any(e => e.StartsWith(beginMessage)))
            {
                throw new ModelErrorAssertionException(string.Format("Expected error message for key '{0}' to start with '{1}', but instead found '{2}'.", _errorKey, beginMessage, string.Join(", ", _errors)));
            }
            return _modelTest;
        }

        public ModelTest<TModel> EndingWith(string endMessage)
        {
            if (!_errors.Any(e => e.EndsWith(endMessage)))
            {
                throw new ModelErrorAssertionException(string.Format("Expected error message for key '{0}' to end with '{1}', but instead found '{2}'.", _errorKey, endMessage, string.Join(", ", _errors)));
            }
            return _modelTest;
        }

        public ModelTest<TModel> Containing(string containsMessage)
        {
            if (!_errors.Any(e => e.Contains(containsMessage)))
            {
                throw new ModelErrorAssertionException(string.Format("Expected error message for key '{0}' to contain '{1}', but instead found '{2}'.", _errorKey, containsMessage, string.Join(", ", _errors)));
            }
            return _modelTest;
        }

        public ModelErrorTest<TModel> AndModelErrorFor<TAttribute>(Expression<Func<TModel, TAttribute>> memberWithError)
        {
            return _modelTest.AndModelErrorFor(memberWithError);
        }

        public ModelErrorTest<TModel> AndModelError(string errorKey)
        {
            return _modelTest.AndModelError(errorKey);
        }
    }

    #endregion

    #region Exceptions

    public class ActionResultAssertionException : Exception
    {
        public ActionResultAssertionException(string message) : base(message) { }
    }

    public class ViewResultModelAssertionException : Exception
    {
        public ViewResultModelAssertionException(string message) : base(message) { }
    }

    public class ModelErrorAssertionException : Exception
    {
        public ModelErrorAssertionException(string message) : base(message) { }
    }

    public class InvalidControllerActionException : Exception
    {
        public InvalidControllerActionException(string message) : base(message) { }
    }

    #endregion
}
