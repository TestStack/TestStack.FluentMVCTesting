using System.IO;
using System.Text;
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
        public static byte[] BinaryFileContents = { 1 };
        public static string TextualFileContent = "textual content";
        public static byte[] EmptyFileBuffer = {  };
        public static readonly Stream EmptyStreamContents = new MemoryStream(EmptyFileBuffer);
        public static readonly Stream BinaryStreamContents = new MemoryStream(BinaryFileContents);
        public const string TextualContent = "textual content";
        public static readonly Encoding TextualContentEncoding = Encoding.UTF8;
        public const string ContentType = "application/contentType";
        #endregion

        #region Empty, Null and Random Results
        public EmptyResult EmptyResult()
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

        #region Files
        public ActionResult EmptyFile()
        {
            var content = new byte[] { };
            return File(content, FileContentType);
        }

        public ActionResult BinaryFile()
        {
            return File(BinaryFileContents, FileContentType);
        }

        public ActionResult TextualFile()
        {
            return TextualFile(Encoding.UTF8);
        }

        public ActionResult TextualFile(Encoding encoding)
        {
            var encodedContents = encoding.GetBytes(TextualFileContent);
            return File(encodedContents, FileContentType);
        }

        public ActionResult EmptyStream()
        {
            return File(EmptyStreamContents, FileContentType);
        }

        public ActionResult BinaryStream()
        {
            return File(BinaryStreamContents, FileContentType);
        }

        public ActionResult TextualStream()
        {
            return TextualStream(Encoding.UTF8);
        }

        public ActionResult TextualStream(Encoding encoding)
        {
            var encondedContent = encoding.GetBytes(TextualFileContent);
            var stream = new MemoryStream(encondedContent);
            return File(stream, FileContentType);
        }

        public ActionResult EmptyFilePath()
        {
            return File(FileName, FileContentType);
        }
        #endregion

        #region Http Status
        public ActionResult NotFound()
        {
            return HttpNotFound();
        }
        public HttpStatusCodeResult StatusCode()
        {
            return new HttpStatusCodeResult(Code);
        }
        #endregion

        #region JSON
        public JsonResult Json()
        {
            return Json(JsonValue);
        }
        #endregion

        #region Content
        
        public ActionResult Content()
        {
            return Content(TextualContent, ContentType, TextualContentEncoding);
        }

        public ActionResult ContentWithoutEncodingSpecified()
        {
            return Content(TextualContent, ContentType);
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
