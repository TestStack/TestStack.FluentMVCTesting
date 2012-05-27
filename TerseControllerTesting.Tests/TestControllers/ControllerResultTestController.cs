using System.Web.Mvc;

namespace TerseControllerTesting.Tests.TestControllers
{
    class ControllerResultTestController : Controller
    {
        #region Empty, Null and Random Results
        public ActionResult EmptyResult()
        {
            return new EmptyResult();
        }

        public ActionResult Null()
        {
            return null;
        }

        public ActionResult RandomResult()
        {
            return new RandomResult();
        }
        #endregion

        #region Redirects
        public ActionResult RedirectToUrl()
        {
            return Redirect("http://url/");
        }

        public ActionResult RedirectToRouteName()
        {
            return RedirectToRoute("RouteName");
        }

        public ActionResult RedirectToActionWithNoParameters()
        {
            return RedirectToAction("ActionWithNoParameters");
        }

        public ActionResult RedirectToActionWithOneParameter()
        {
            return RedirectToAction("ActionWithOneParameter");
        }

        public ActionResult RedirectToActionWithTwoParameters()
        {
            return RedirectToAction("ActionWithTwoParameters");
        }

        public ActionResult RedirectToActionWithThreeParameters()
        {
            return RedirectToAction("ActionWithThreeParameters");
        }

        public ActionResult RedirectToActionWithMoreThanThreeParameters()
        {
            return RedirectToAction("ActionWithMoreThanThreeParameters");
        }

        public ActionResult RedirectToAnotherController()
        {
            return RedirectToAction("SomeAction", "SomeOtherController");
        }
        #endregion

        #region Redirect Actions
        public ActionResult ActionWithNoParameters()
        {
            return new EmptyResult();
        }
        public ActionResult ActionWithOneParameter(int param1)
        {
            return new EmptyResult();
        }
        public ActionResult ActionWithTwoParameters(int param1, int param2)
        {
            return new EmptyResult();
        }
        public ActionResult ActionWithThreeParameters(int param1, int param2, int param3)
        {
            return new EmptyResult();
        }
        public ActionResult ActionWithMoreThanThreeParameters(int param1, int param2, int param3)
        {
            return new EmptyResult();
        }
        #endregion

        #region Views
        public ActionResult DefaultView()
        {
            return View();
        }

        public ActionResult DefaultPartial()
        {
            return PartialView();
        }

        public ActionResult EmptyFile()
        {
            var content = new byte[] {};
            return File(content, "application/contentType");
        }

        public ActionResult NamedView()
        {
            return View("NamedView");
        }

        public ActionResult NamedPartial()
        {
            return PartialView("NamedPartial");
        }
        #endregion

        #region Http Status
        public ActionResult NotFound()
        {
            return HttpNotFound();
        }
        public ActionResult StatusCode()
        {
            return new HttpStatusCodeResult(403);
        }
        #endregion

        #region JSON
        public ActionResult Json()
        {
            return Json("{data:true}");
        }
        #endregion
    }

    class SomeOtherController : Controller
    {
        public ActionResult SomeAction()
        {
            return new EmptyResult();
        }
    }

    class RandomResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context) {}
    }
}
