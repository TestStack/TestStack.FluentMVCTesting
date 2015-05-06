using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public class SimpleActionExecutor : IActionExecutor
    {
        public ControllerResultTest<TController> Execute<TController, TAction>(TController controller, Expression<Func<TController, TAction>> actionCall)
            where TController : Controller
            where TAction : ActionResult
        {
            var actionName = ((MethodCallExpression)actionCall.Body).Method.Name;

            var actionResult = actionCall.Compile().Invoke(controller);

            return new ControllerResultTest<TController>(controller, actionName, actionResult);
        }

        public ControllerResultTest<TController> Execute<TController, TAction>(TController controller, Expression<Func<TController, Task<TAction>>> actionCall)
            where TController : Controller
            where TAction : ActionResult
        {
            var actionName = ((MethodCallExpression)actionCall.Body).Method.Name;

            var actionResult = actionCall.Compile().Invoke(controller).Result;

            return new ControllerResultTest<TController>(controller, actionName, actionResult);
        }
    }
}