using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    partial class ControllerResultTestShould
    {
        [Test]
        public void Allow_the_object_that_is_returned_to_be_checked()
        {
            _controller.WithCallTo(c => c.Json())
                .ShouldReturnJson(d => Assert.That(d, Is.EqualTo(ControllerResultTestController.JsonValue)));
        }

        [Test]
        public void Return_the_json_result()
        {
            var expected = _controller.Json();
            var actual = _controller.WithCallTo(c => c.Json()).ShouldReturnJson();
            Assert.That(actual.Data, Is.EqualTo(expected.Data));
            Assert.That(actual.JsonRequestBehavior, Is.EqualTo(expected.JsonRequestBehavior));
        }

        [Test]
        public void Return_the_json_result_when_the_assertion_is_true()
        {
            var expected = _controller.Json();
            var actual =_controller.WithCallTo(c => c.Json())
                .ShouldReturnJson(d => Assert.That(d, Is.EqualTo(ControllerResultTestController.JsonValue)));
            Assert.That(actual.Data, Is.EqualTo(expected.Data));
            Assert.That(actual.JsonRequestBehavior, Is.EqualTo(expected.JsonRequestBehavior));
        }
    }
}