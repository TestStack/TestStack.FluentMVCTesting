using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        public EmptyResult ShouldReturnEmptyResult()
        {
            ValidateActionReturnType<EmptyResult>();
            return (EmptyResult) ActionResult;
        }
    }
}