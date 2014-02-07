using System;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting.Sample.Controllers
{
    public enum UserType
    {
        Unknown = 0
    }

    public class ProductController : Controller
    {
        public ActionResult Product()
        {
            return View();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Product", new { Id = 1 });
        }

        public ActionResult User()
        {
            return View();
        }

        public ActionResult UnknownUser()
        {
            return RedirectToAction("User", new { Id = Guid.Empty, UserType = UserType.Unknown });
        }
    }
}