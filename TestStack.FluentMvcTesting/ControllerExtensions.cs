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
                throw new InvalidControllerActionException("Expected action \{action.Name} of controller \{controller.GetType().Name} to be a child action, but it didn't have the ChildActionOnly attribute.");

            return controller.WithCallTo(actionCall);
        }

        public static ControllerResultTest<T> WithCallToChild<T, TAction>(this T controller, Expression<Func<T, Task<TAction>>> actionCall)
            where T : Controller
            where TAction : ActionResult
        {
            var action = ((MethodCallExpression)actionCall.Body).Method;

            if (!action.IsDefined(typeof(ChildActionOnlyAttribute), false))
                throw new InvalidControllerActionException("Expected action \{action.Name} of controller \{controller.GetType().Name} to be a child action, but it didn't have the ChildActionOnly attribute.");

            return controller.WithCallTo(actionCall);
        }

        public static TempDataResultTest ShouldHaveTempDataProperty(this ControllerBase controller, string key, object value = null)
        {
            var actual = controller.TempData[key];

            if (actual == null)
            {
                throw new TempDataAssertionException("Expected TempData to have a non-null value with key \"\{key}\", but none found.");
            }

            if (value != null && actual.GetType() != value.GetType())
            {
                throw new TempDataAssertionException("Expected value to be of type \{value.GetType().FullName}, but instead was \{actual.GetType().FullName}.");
            }

            if (value != null && !value.Equals(actual))
            {
                throw new TempDataAssertionException("Expected value for key \"\{key}\" to be \"\{value}\", but instead found \"\{actual}\"");
            }

            return new TempDataResultTest(controller);
        }

        public static TempDataResultTest ShouldHaveTempDataProperty<TValue>(this ControllerBase controller, string key, Func<TValue, bool> predicate)
        {
            var actual = controller.TempData[key];

            if (actual == null)
            {
                throw new TempDataAssertionException("Expected TempData to have a non-null value with key \"\{key}\", but none found.");
            }

            if (!predicate((TValue)actual))
            {
                throw new TempDataAssertionException("Expected view model to pass the given condition, but it failed.");
            }

            return new TempDataResultTest(controller);
        }

        public static TempDataResultTest ShouldNotHaveTempDataProperty(this Controller controller, string key)
        {
            var actual = controller.TempData[key];

            if (actual != null)
            {
                throw new TempDataAssertionException("Expected TempData to have no value with key \"\{key}\", but found one.");
            }

            return new TempDataResultTest(controller);
        }

    }
}
