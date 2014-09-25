using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    partial class ControllerResultTestShould
    {
        [Test]
        public void Allow_the_object_that_is_returned_to_be_checked()
        {
            _controller.WithCallTo(c => c.Json()).ShouldReturnJson(d => Assert.That(d, Is.EqualTo(ControllerResultTestController.JsonValue)));
        }
    }
}