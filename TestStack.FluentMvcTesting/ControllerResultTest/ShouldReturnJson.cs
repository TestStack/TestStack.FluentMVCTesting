using System;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        public JsonResult ShouldReturnJson()
        {
            ValidateActionReturnType<JsonResult>();
            return (JsonResult) ActionResult;
        }

        public JsonResult ShouldReturnJson(Action<dynamic> assertion)
        {
            ValidateActionReturnType<JsonResult>();
            var jsonResult = (JsonResult)ActionResult;
            assertion(jsonResult.Data);
            return (JsonResult)ActionResult;
        }
    }
}