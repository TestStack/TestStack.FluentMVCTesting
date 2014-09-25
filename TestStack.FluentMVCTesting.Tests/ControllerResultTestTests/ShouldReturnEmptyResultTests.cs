using NUnit.Framework;

namespace TestStack.FluentMVCTesting.Tests
{
    partial class ControllerResultTestShould
    {
        [Test]
        public void Check_for_empty_result()
        {
            _controller.WithCallTo(c => c.EmptyResult()).ShouldReturnEmptyResult();
        }
    }
}