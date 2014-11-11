using System;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        public void ShouldReturnJson()
        {
            ValidateActionReturnType<JsonResult>();
        }

        public void ShouldReturnJson(Action<dynamic> assertion)
        {
            ValidateActionReturnType<JsonResult>();
            var jsonResult = (JsonResult)ActionResult;
            assertion(jsonResult.Data);
        }
    }
}