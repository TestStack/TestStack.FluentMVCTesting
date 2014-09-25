using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        private ViewResultTest ShouldRenderViewResult<TViewResult>(string viewName) where TViewResult : ViewResultBase
        {
            ValidateActionReturnType<TViewResult>();

            var viewResult = (TViewResult)_actionResult;

            if (viewResult.ViewName != viewName && (viewName != _actionName || viewResult.ViewName != ""))
            {
                throw new ActionResultAssertionException(string.Format("Expected result view to be '{0}', but instead was given '{1}'.", viewName, viewResult.ViewName == "" ? _actionName : viewResult.ViewName));
            }

            return new ViewResultTest(viewResult, _controller);
        }

        public ViewResultTest ShouldRenderView(string viewName)
        {
            return ShouldRenderViewResult<ViewResult>(viewName);
        }

        public ViewResultTest ShouldRenderPartialView(string viewName)
        {
            return ShouldRenderViewResult<PartialViewResult>(viewName);
        }

        public ViewResultTest ShouldRenderDefaultView()
        {
            return ShouldRenderView(_actionName);
        }

        public ViewResultTest ShouldRenderDefaultPartialView()
        {
            return ShouldRenderPartialView(_actionName);
        }
    }
}