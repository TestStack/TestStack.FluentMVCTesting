using System.IO;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting.Tests.TestControllers
{
    class ControllerResultTestController : Controller
    {
        #region Testing Constants
        public const string RouteName = "RouteName";
        public const string RedirectUrl = "http://url/";
        public const string FileContentType = "application/contentType";
        public const string ViewName = "NamedView";
        public const string PartialName = "NamedPartial";
        public const string RandomViewName = "Random";
        public const int Code = 403;
        public const string JsonValue = "json";
        public const string FileName = "NamedFile";
        #endregion

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

        public ActionResult RedirectToRandomResult()
        {
            return RedirectToAction("RandomResult");
        }
        #endregion

        #region Redirects
        public ActionResult RedirectToUrl()
        {
            return Redirect(RedirectUrl);
        }

        public ActionResult RedirectToRouteName()
        {
            return RedirectToRoute(RouteName);
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
            return RedirectToAction("SomeAction", "SomeOther");
        }

        public ActionResult RedirectToAnotherActionNoController()
        {
            return RedirectToAction("SomeAction");
        }

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
        public ActionResult ActionWithMoreThanThreeParameters(int param1, int param2, int param3, int param4)
        {
            return new EmptyResult();
        }
        #endregion

        #endregion

        #region Views
        public ActionResult DefaultView()
        {
            return View();
        }

        public ActionResult DefaultViewExplicit()
        {
            return View("DefaultViewExplicit");
        }

        public ActionResult DefaultPartial()
        {
            return PartialView();
        }

        public ActionResult DefaultPartialExplicit()
        {
            return PartialView("DefaultPartialExplicit");
        }

        public ActionResult EmptyFile()
        {
            var content = new byte[] {};
            return File(content, FileContentType);
        }

        public ActionResult EmptyStream()
        {
            var content = new MemoryStream();
            return File(content, FileContentType);
        }

        public ActionResult EmptyFilePath()
        {
            return File(FileName, FileContentType);
        }

        public ActionResult NamedView()
        {
            return View(ViewName);
        }

        public ActionResult NamedPartial()
        {
            return PartialView(PartialName);
        }

        public ActionResult RandomView()
        {
            return View(RandomViewName);
        }

        public ActionResult RandomPartial()
        {
            return PartialView(RandomViewName);
        }
        #endregion

        #region Http Status
        public ActionResult NotFound()
        {
            return HttpNotFound();
        }
        public ActionResult StatusCode()
        {
            return new HttpStatusCodeResult(Code);
        }
        #endregion

        #region JSON
        public ActionResult Json()
        {
            return Json(JsonValue);
        }
        #endregion
    }

    #region Test Classes
    class SomeOtherController : Controller
    {
        public ActionResult SomeAction()
        {
            return new EmptyResult();
        }

        public ActionResult SomeOtherAction()
        {
            return new EmptyResult();
        }
    }

    class YetAnotherController : Controller
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
    #endregion
}
