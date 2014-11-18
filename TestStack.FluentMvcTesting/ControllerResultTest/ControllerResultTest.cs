using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T> where T : Controller
    {
        public T Controller { get; }
        public string ActionName { get; }
        public ActionResult ActionResult { get; }

        public void ValidateActionReturnType<TActionResult>() where TActionResult : ActionResult
        {
            var castedActionResult = ActionResult as TActionResult;

            if (ActionResult == null)
                throw new ActionResultAssertionException(string.Format("Received null action result when expecting {0}.", 
                    typeof(TActionResult).Name));

            if (castedActionResult == null)
                throw new ActionResultAssertionException(string.Format("Expected action result to be a {0}, but instead received a {1}.",
                    typeof(TActionResult).Name, 
                    ActionResult.GetType().Name));
        }

        public ControllerResultTest(T controller, string actionName, ActionResult actionResult)
        {
            Controller = controller;
            ActionName = actionName;
            ActionResult = actionResult;
        }
    }
}