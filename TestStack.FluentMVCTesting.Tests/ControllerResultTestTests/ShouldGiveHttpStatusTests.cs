using System.Net;
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
    }
}