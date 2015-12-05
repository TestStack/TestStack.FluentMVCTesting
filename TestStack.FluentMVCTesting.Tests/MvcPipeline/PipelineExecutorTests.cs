using System.Web.Mvc;
using NUnit.Framework;
using TestStack.FluentMVCTesting.MvcPipeline;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests.MvcPipeline
{
    [TestFixture]
    class PipelineExecutorTests
    {
        [Test]
        public void should_call_filters()
        {
            var controller = new ControllerExtensionsController();
            GlobalFilters.Filters.Add(new DummyFilter());
            var executor = new PipelineActionExecutor();

            var result = executor.Execute(controller, c => c.SomeAction());

            Assert.That(DummyFilter.LoggingFilterWasCalled);
        }

        private class DummyFilter : ActionFilterAttribute, IActionFilter
        {
            public static bool LoggingFilterWasCalled { get; set; }

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                LoggingFilterWasCalled = true;
            }
        }
    }

}
