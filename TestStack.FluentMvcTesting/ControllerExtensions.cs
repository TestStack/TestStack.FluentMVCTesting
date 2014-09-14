using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
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

        public static ControllerResultTest<T> WithCallTo<T, TAction>(this T controller, Expression<Func<T, Task<TAction>>> actionCall)
            where T : Controller
            where TAction : ActionResult
        {
            var actionName = ((MethodCallExpression)actionCall.Body).Method.Name;

            var actionResult = actionCall.Compile().Invoke(controller).Result;

            return new ControllerResultTest<T>(controller, actionName, actionResult);
        }

        public static ControllerResultTest<T> WithCallToChild<T, TAction>(this T controller, Expression<Func<T, TAction>> actionCall)
            where T : Controller
            where TAction : ActionResult
        {
            var action = ((MethodCallExpression)actionCall.Body).Method;

            if (!action.IsDefined(typeof(ChildActionOnlyAttribute), false))
                throw new InvalidControllerActionException(string.Format("Expected action {0} of controller {1} to be a child action, but it didn't have the ChildActionOnly attribute.", action.Name, controller.GetType().Name));

            return controller.WithCallTo(actionCall);
        }

        public static ControllerResultTest<T> WithCallToChild<T, TAction>(this T controller, Expression<Func<T, Task<TAction>>> actionCall)
            where T : Controller
            where TAction : ActionResult
        {
            var action = ((MethodCallExpression)actionCall.Body).Method;

            if (!action.IsDefined(typeof(ChildActionOnlyAttribute), false))
                throw new InvalidControllerActionException(string.Format("Expected action {0} of controller {1} to be a child action, but it didn't have the ChildActionOnly attribute.", action.Name, controller.GetType().Name));

            return controller.WithCallTo(actionCall);
        }

        public static TempDataResultTest ShouldHaveTempDataProperty(this ControllerBase controller, string key, object value = null)
        {
            var actual = controller.TempData[key];

            if (actual == null)
            {
                throw new TempDataAssertionException(string.Format(
                    "Expected TempData to have a non-null value with key \"{0}\", but none found.", key));
            }

            if (value != null && actual.GetType() != value.GetType())
            {
                throw new TempDataAssertionException(string.Format(
                    "Expected value to be of type {0}, but instead was {1}.",
                    value.GetType().FullName,
                    actual.GetType().FullName));
            }

            if (value != null && !value.Equals(actual))
            {
                throw new TempDataAssertionException(string.Format(
                    "Expected value for key \"{0}\" to be \"{1}\", but instead found \"{2}\"", key, value, actual));
            }

            return new TempDataResultTest(controller);
        }

        public static TempDataResultTest ShouldHaveTempDataProperty<TValue>(this ControllerBase controller, string key, Func<TValue, bool> predicate)
        {
            var actual = controller.TempData[key];

            if (actual == null)
            {
                throw new TempDataAssertionException(string.Format(
                    "Expected TempData to have a non-null value with key \"{0}\", but none found.", key));
            }

            if (!predicate((TValue)actual))
            {
                throw new TempDataAssertionException("Expected view model to pass the given condition, but it failed.");
            }

            return new TempDataResultTest(controller);
        }
    }
}
