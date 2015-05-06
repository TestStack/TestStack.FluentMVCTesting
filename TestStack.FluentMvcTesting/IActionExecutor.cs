using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public interface IActionExecutor
    {
        ControllerResultTest<TController> Execute<TController, TAction>(TController controller, Expression<Func<TController, TAction>> actionCall)
            where TController : Controller
            where TAction : ActionResult;

        ControllerResultTest<TController> Execute<TController, TAction>(TController controller, Expression<Func<TController, Task<TAction>>> actionCall)
            where TController : Controller
            where TAction : ActionResult;
    }
}