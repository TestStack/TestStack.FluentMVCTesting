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
                var actual = viewResult.ViewName == "" ? ActionName : viewResult.ViewName;
                throw new ActionResultAssertionException("Expected result view to be '\{viewName}', but instead was given '\{actual}'.");
            }

            return new ViewResultTest(viewResult, Controller);
        }

        public ViewResultTest ShouldRenderView(string viewName) => ShouldRenderViewResult<ViewResult>(viewName);
        public ViewResultTest ShouldRenderPartialView(string viewName) => ShouldRenderViewResult<PartialViewResult>(viewName);
        public ViewResultTest ShouldRenderDefaultView() => ShouldRenderView(ActionName);
        public ViewResultTest ShouldRenderDefaultPartialView() => ShouldRenderPartialView(ActionName);
    }
}