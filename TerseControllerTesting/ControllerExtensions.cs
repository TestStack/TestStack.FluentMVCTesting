using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace TerseControllerTesting
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
}
