using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    internal class MvcActionDescriptor : ReflectedActionDescriptor
    {
        private MvcActionDescriptor(MethodCallExpression methodCallExpression, ReflectedControllerDescriptor controllerDescriptor)
            : base(methodCallExpression.Method, methodCallExpression.Method.Name, controllerDescriptor)
        {
        }

        public static ActionDescriptor Create<TController,TActionResult>(Expression<Func<TController, TActionResult>> actionExpression)
            where TController : ControllerBase
            where TActionResult : ActionResult
        {
            var methodCallExpression = Reflector.GetMethodCallExpression(actionExpression.Body);

            return new MvcActionDescriptor(methodCallExpression, new ReflectedControllerDescriptor(typeof(TController)));
        }

        public static ActionDescriptor Create<TController, TActionResult>(Expression<Func<TController, Task<TActionResult>>> actionExpression)
            where TController : ControllerBase
            where TActionResult : ActionResult
        {
            var methodCallExpression = Reflector.GetMethodCallExpression(actionExpression.Body);

            return new MvcActionDescriptor(methodCallExpression, new ReflectedControllerDescriptor(typeof(TController)));
        }
    }
}
