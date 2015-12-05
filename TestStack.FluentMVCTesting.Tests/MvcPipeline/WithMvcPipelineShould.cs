using System.Web.Mvc;
using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests.MvcPipeline
{
#if NET45
    [TestFixture]
    class WithMvcPipelineShould
    {
        [Test]
        public void Call_action()
        {
            var controller = new ControllerExtensionsController();
            controller.WithMvcPipelineTo(c => c.SomeAction());
            Assert.That(controller.SomeActionCalled);
        }

        [Test]
        public void Can_exercise_filters()
        {
            GlobalFilters.Filters.Add(new LoggingFilter());
            var controller = new ControllerExtensionsController();
            
            controller.WithMvcPipelineTo(c => c.SomeAction());
            
            Assert.That(LoggingFilter.OnActionExecutingRan);
            Assert.That(LoggingFilter.OnActionExecutedRan);
        }

        private class LoggingFilter : ActionFilterAttribute, IActionFilter
        {
            public static bool OnActionExecutingRan;
            public static bool OnActionExecutedRan;

            public LoggingFilter()
            {
                OnActionExecutingRan = false;
                OnActionExecutedRan = false;
            }
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                OnActionExecutingRan = true;
            }

            public override void OnActionExecuted(ActionExecutedContext filterContext)
            {
                OnActionExecutedRan = true;
            }
        }
    }
#endif
}
