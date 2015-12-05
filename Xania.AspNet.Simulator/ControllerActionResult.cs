using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Xania.AspNet.Simulator
{
    public class ControllerActionResult
    {
        public ControllerContext ControllerContext { get; set; }
        public ActionResult ActionResult { get; set; }

        public dynamic ViewBag
        {
            get { return ControllerContext.Controller.ViewBag; }
        }

        public ViewDataDictionary ViewData
        {
            get { return ControllerContext.Controller.ViewData; }
        }

        public ModelStateDictionary ModelState
        {
            get { return ControllerContext.Controller.ViewData.ModelState; }
        }

        public HttpResponseBase Response
        {
            get { return ControllerContext.HttpContext.Response; }
        }

        public ControllerBase Controller
        {
            get { return ControllerContext.Controller; }
        }

        public HttpRequestBase Request
        {
            get { return ControllerContext.HttpContext.Request; }
        }

        public void ExecuteResult()
        {
            ActionResult.ExecuteResult(ControllerContext);
        }
    }
}