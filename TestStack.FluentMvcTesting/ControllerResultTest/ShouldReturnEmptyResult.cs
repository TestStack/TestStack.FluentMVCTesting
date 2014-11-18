using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        public void ShouldReturnEmptyResult() => ValidateActionReturnType<EmptyResult>();
    }
}