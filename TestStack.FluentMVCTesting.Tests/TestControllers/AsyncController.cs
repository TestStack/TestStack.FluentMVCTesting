using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting.Tests.TestControllers
{
    class AsyncController : Controller
    {
        public Task<ActionResult> AsyncViewAction()
        {
            // Task.FromResult would be better, but I want to support .NET 4
            return Task.Factory.StartNew<ActionResult>(() => View());
        }

        [ChildActionOnly]
        public Task<ActionResult> AsyncChildViewAction()
        {
            return Task.Factory.StartNew<ActionResult>(() => View());
        }
    }
}
