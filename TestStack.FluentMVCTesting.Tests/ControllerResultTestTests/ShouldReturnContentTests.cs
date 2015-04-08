using NUnit.Framework;
using System.Web.Mvc;
using System.Text;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    partial class ControllerResultTestShould
    {
        [Test]
        public void Check_for_content_result()
        {
            _controller.WithCallTo(c => c.Content()).ShouldReturnContent();
        }

        [Test]
        public void Check_for_content_result_and_check_content()
        {
            _controller.WithCallTo(c => c.Content()).ShouldReturnContent(ControllerResultTestController.TextualContent);
        }

        [Test]
        public void Check_for_content_result_and_check_invalid_content()
        {
            const string content = "dummy contents";

            var exception = Assert.Throws<ActionResultAssertionException>(() => _controller.WithCallTo(c => c.Content()).ShouldReturnContent(content));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected content to be '{0}', but instead was '{1}'.", content, ControllerResultTestController.TextualContent)));
        }

        [Test]
        public void Check_for_content_result_and_check_content_and_check_content_type()
        {
            _controller.WithCallTo(c => c.Content()).ShouldReturnContent(ControllerResultTestController.TextualContent, ControllerResultTestController.ContentType);
        }

        [Test]
        public void Check_for_content_result_and_check_content_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() => _controller.WithCallTo(c => c.Content()).ShouldReturnContent(ControllerResultTestController.TextualContent, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected content type to be '{0}', but instead was '{1}'.", contentType, ControllerResultTestController.ContentType)));
        }

        [Test]
        public void Check_for_content_result_and_check_content_and_check_content_type_and_check_content_encoding()
        {
            _controller.WithCallTo(c => c.Content()).ShouldReturnContent(ControllerResultTestController.TextualContent, ControllerResultTestController.ContentType, ControllerResultTestController.TextualContentEncoding);
        }

        [Test]
        public void Check_for_content_result_and_check_content_and_check_content_type_and_check_invalid_content_encoding()
        {
            var encoding = Encoding.Unicode;

            var exception = Assert.Throws<ActionResultAssertionException>(() => _controller.WithCallTo(c => c.Content()).ShouldReturnContent(ControllerResultTestController.TextualContent, ControllerResultTestController.ContentType, encoding));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected encoding to be equal to {0}, but instead was {1}.", encoding.EncodingName, ControllerResultTestController.TextualContentEncoding.EncodingName)));
        }

        [Test]
        public void Check_for_content_result_and_check_invalid_content_and_check_invalid_content_type_and_check_invalid_encoding()
        {
            const string contentType = "application/dummy";
            const string content = "dumb";
            Encoding encoding = Encoding.Unicode;

            var exception = Assert.Throws<ActionResultAssertionException>(() => _controller.WithCallTo(c => c.Content()).ShouldReturnContent(content, contentType, encoding));

            // Assert that the content type validation occurs before that of the actual content.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Emit_readable_error_message_when_the_actual_content_encoding_has_not_been_specified()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() => _controller.WithCallTo(c => c.ContentWithoutEncodingSpecified()).ShouldReturnContent(encoding: ControllerResultTestController.TextualContentEncoding));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected encoding to be equal to {0}, but instead was null.", ControllerResultTestController.TextualContentEncoding.EncodingName)));
        }

        [Test]
        public void Return_the_content_result()
        {
            var expected = (ContentResult)_controller.Content();
            var actual = _controller.WithCallTo(c => c.Content())
                .ShouldReturnContent(ControllerResultTestController.TextualContent);
            Assert.That(actual.Content,Is.EqualTo(expected.Content));
            Assert.That(actual.ContentType, Is.EqualTo(expected.ContentType));
            Assert.That(actual.ContentEncoding, Is.EqualTo(expected.ContentEncoding));
        }
    }
}