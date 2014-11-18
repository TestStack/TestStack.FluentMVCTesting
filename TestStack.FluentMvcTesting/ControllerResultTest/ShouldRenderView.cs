using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        private ViewResultTest ShouldRenderViewResult<TViewResult>(string viewName) where TViewResult : ViewResultBase
        {
            ValidateActionReturnType<TViewResult>();

            var viewResult = (TViewResult)ActionResult;

            if (viewResult.ViewName != viewName && (viewName != ActionName || viewResult.ViewName != ""))
            {
                throw new ActionResultAssertionException(string.Format("Expected result view to be '{0}', but instead was given '{1}'.", viewName, viewResult.ViewName == "" ? ActionName : viewResult.ViewName));
            }

            return new ViewResultTest(viewResult, Controller);
        }

        public ViewResultTest ShouldRenderView(string viewName) => ShouldRenderViewResult<ViewResult>(viewName);
        public ViewResultTest ShouldRenderPartialView(string viewName) => ShouldRenderViewResult<PartialViewResult>(viewName);
        public ViewResultTest ShouldRenderDefaultView() => ShouldRenderView(ActionName);
        public ViewResultTest ShouldRenderDefaultPartialView() => ShouldRenderPartialView(ActionName);
    }
}