using System.Web.Mvc;

namespace Xania.AspNet.Simulator
{
    public class ActionContext
    {
        public ControllerContext ControllerContext { get; set; }
        public ActionDescriptor ActionDescriptor { get; set; }
    }
}