using System.Net;
using System.Web.Mvc;
using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    partial class ControllerResultTestShould
    {
        [Test]
        public void Check_for_http_not_found()
        {
            _controller.WithCallTo(c => c.NotFound()).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [Test]
        public void Check_for_http_status()
        {
            _controller.WithCallTo(c => c.StatusCode()).ShouldGiveHttpStatus(ControllerResultTestController.Code);
        }

        [Test]
        public void Check_for_invalid_http_status()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.StatusCode()).ShouldGiveHttpStatus(ControllerResultTestController.Code + 1)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected HTTP status code to be '{0}', but instead received a '{1}'.", ControllerResultTestController.Code + 1, ControllerResultTestController.Code)));
        }

        [Test]
        public void Return_the_http_status_result()
        {
            HttpStatusCodeResult expected = _controller.StatusCode();
            HttpStatusCodeResult actual = _controller.WithCallTo(c => c.StatusCode())
                .ShouldGiveHttpStatus();
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            Assert.AreEqual(expected.StatusDescription, actual.StatusDescription);
        }

        [Test]
        public void Reeturn_the_http_status_result_when_the_assertion_against_integer_is_true()
        {
            HttpStatusCodeResult expected = _controller.StatusCode();
            HttpStatusCodeResult actual = _controller.WithCallTo(c => c.StatusCode())
                .ShouldGiveHttpStatus(ControllerResultTestController.Code);
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            Assert.AreEqual(expected.StatusDescription, actual.StatusDescription);
        }

        [Test]
        public void Reeturn_the_http_status_result_when_the_assertion_against_status_code_enum_is_true()
        {
            HttpStatusCodeResult expected = _controller.StatusCode();
            HttpStatusCodeResult actual = _controller.WithCallTo(c => c.StatusCode())
                .ShouldGiveHttpStatus((HttpStatusCode) ControllerResultTestController.Code);
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            Assert.AreEqual(expected.StatusDescription, actual.StatusDescription);
        }
    }
}