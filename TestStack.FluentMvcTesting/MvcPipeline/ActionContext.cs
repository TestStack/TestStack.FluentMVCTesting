using System.Web.Mvc;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    public class ActionContext
    {
        public ControllerContext ControllerContext { get; set; }
        public ActionDescriptor ActionDescriptor { get; set; }
    }
}
