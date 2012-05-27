using System.Web.Mvc;

namespace TerseControllerTesting.Tests.TestControllers
{
    class ControllerExtensionsController : Controller
    {
        public bool SomeActionCalled { get; set; }
        public bool SomeChildActionCalled { get; set; }

        public ActionResult SomeAction()
        {
            SomeActionCalled = true;
            return new EmptyResult();
        }

        [ChildActionOnly]
        public ActionResult SomeChildAction()
        {
            SomeChildActionCalled = true;
            return new EmptyResult();
        }
    }
}
