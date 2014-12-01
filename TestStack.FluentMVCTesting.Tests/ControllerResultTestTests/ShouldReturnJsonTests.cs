using System.Web.Mvc;
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
            JsonResult expected = _controller.Json();
            JsonResult actual = _controller.WithCallTo(c => c.Json()).ShouldReturnJson();
            Assert.AreEqual(expected.Data, actual.Data);
            Assert.AreEqual(expected.JsonRequestBehavior, actual.JsonRequestBehavior);
        }

        [Test]
        public void Return_the_json_result_when_the_assertion_is_true()
        {
            JsonResult expected = _controller.Json();
            JsonResult actual =_controller.WithCallTo(c => c.Json())
                .ShouldReturnJson(d => Assert.That(d, Is.EqualTo(ControllerResultTestController.JsonValue)));
            Assert.AreEqual(expected.Data, actual.Data);
            Assert.AreEqual(expected.JsonRequestBehavior, actual.JsonRequestBehavior);
        }
    }
}