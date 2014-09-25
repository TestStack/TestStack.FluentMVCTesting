using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T> where T : Controller
    {
        private readonly T _controller;
        private readonly string _actionName;
        private readonly ActionResult _actionResult;

        private void ValidateActionReturnType<TActionResult>() where TActionResult : ActionResult
        {
            var castedActionResult = _actionResult as TActionResult;

            if (_actionResult == null)
                throw new ActionResultAssertionException(string.Format("Received null action result when expecting {0}.", typeof(TActionResult).Name));

            if (castedActionResult == null)
                throw new ActionResultAssertionException(
                    string.Format("Expected action result to be a {0}, but instead received a {1}.",
                                  typeof(TActionResult).Name, _actionResult.GetType().Name
                        )
                    );
        }

        public ControllerResultTest(T controller, string actionName, ActionResult actionResult)
        {
            _controller = controller;
            _actionName = actionName;
            _actionResult = actionResult;
        }
    }
}