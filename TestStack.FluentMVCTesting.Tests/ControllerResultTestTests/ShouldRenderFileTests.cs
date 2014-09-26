using System.IO;
using NUnit.Framework;
using System.Text;
using TestStack.FluentMVCTesting.Tests.TestControllers;
using System.Linq;

namespace TestStack.FluentMVCTesting.Tests
{
    partial class ControllerResultTestShould
    {
        [Test]
        public void Check_for_any_file_result()
        {
            _controller.WithCallTo(c => c.EmptyFile()).ShouldRenderAnyFile();
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderAnyFile();
            _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderAnyFile();
        }

        [Test]
        public void Check_for_any_file_result_and_check_content_type()
        {
            _controller.WithCallTo(c => c.EmptyFile()).ShouldRenderAnyFile(ControllerResultTestController.FileContentType);
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderAnyFile(ControllerResultTestController.FileContentType);
            _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderAnyFile(ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_any_file_result_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyFile()).ShouldRenderAnyFile(contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_content_result()
        {
            _controller.WithCallTo(c => c.EmptyFile()).ShouldRenderFileContents();
        }

        [Test]
        public void Check_for_file_content_result_and_check_binary_content()
        {
            _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(ControllerResultTestController.BinaryFileContents);
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_binary_content()
        {
            byte[] contents = { 1, 2 };
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(contents));

            Assert.True(exception.Message.StartsWith("Expected file contents to be equal to ["));
            Assert.True(exception.Message.EndsWith("]."));
            Assert.True(string.Join(", ", contents).All(exception.Message.Contains));
            Assert.True(string.Join(", ", ControllerResultTestController.BinaryFileContents).All(exception.Message.Contains));
        }

        [Test]
        public void Check_for_file_content_result_and_check_binary_content_and_check_content_type()
        {
            _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(ControllerResultTestController.BinaryFileContents, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(ControllerResultTestController.BinaryFileContents, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_binary_content_and_check_invalid_content_type()
        {
            byte[] contents = { 1, 2 };
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(contents, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_contents()
        {
            _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent);
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_textual_contents()
        {
            const string contents = "dummy contents";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(contents));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file contents to be \"{0}\", but instead was \"{1}\".", contents, ControllerResultTestController.TextualFileContent)));
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_and_check_content_result()
        {
            _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_and_check_invalid_content_typet()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_textual_content_and_check_invalid_content_type()
        {
            const string contents = "dummy content";
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(contents, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_using_given_char_encoding()
        {
            var encoding = Encoding.BigEndianUnicode;

            _controller.WithCallTo(c => c.TextualFile(encoding))
                .ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, encoding: encoding);
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_using_given_char_encoding_and_check_content_type()
        {
            var encoding = Encoding.BigEndianUnicode;

            _controller.WithCallTo(c => c.TextualFile(encoding)).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType, encoding);
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_using_invalid_given_char_encoding()
        {
            Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType, Encoding.BigEndianUnicode));
        }

        [Test]
        public void Check_for_file_stream_result()
        {
            _controller.WithCallTo(c => c.EmptyStream())
                .ShouldRenderFileStream();
        }

        [Test]
        public void Check_for_file_stream_result_and_check_stream_content()
        {
            _controller.WithCallTo(c => c.EmptyStream())
                .ShouldRenderFileStream(ControllerResultTestController.EmptyStreamContents);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_stream_content()
        {
            var buffer = new byte[] { 1, 2 };
            var stream = new MemoryStream(buffer);

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderFileStream(stream));

            var expected = string.Format("[{0}]", string.Join(", ", buffer));
            var actual = string.Format("[{0}]", string.Join(", ", ControllerResultTestController.EmptyFileBuffer));
            var message = string.Format("Expected file contents to be equal to {0}, but instead was given {1}.", expected, actual);

            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void Check_for_file_stream_result_with_populated_file_and_check_invalid_stream_content()
        {
            var buffer = new byte[] { 1, 2 };
            var stream = new MemoryStream(buffer);

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryStream()).ShouldRenderFileStream(stream));

            var expected = string.Format("[{0}]", string.Join(", ", buffer));
            var actual = string.Format("[{0}]", string.Join(", ", ControllerResultTestController.BinaryFileContents));
            var message = string.Format("Expected file contents to be equal to {0}, but instead was given {1}.", expected, actual);

            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_content_type()
        {
            _controller.WithCallTo(c => c.EmptyStream())
                .ShouldRenderFileStream(ControllerResultTestController.EmptyStreamContents, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderFileStream(ControllerResultTestController.EmptyStreamContents, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format(
                "Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_stream_content_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";
            var buffer = new byte[] { 1, 2 };
            var stream = new MemoryStream(buffer);

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderFileStream(stream, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_binary_content()
        {
            _controller.WithCallTo(c => c.BinaryStream())
                .ShouldRenderFileStream(ControllerResultTestController.BinaryFileContents);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_binary_content()
        {
            var content = new byte[] { 1, 2 };

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryStream()).ShouldRenderFileStream(content));

            var expected = string.Format("[{0}]", string.Join(", ", content));
            var actual = string.Format("[{0}]", string.Join(", ", ControllerResultTestController.BinaryFileContents));
            var message = string.Format("Expected file contents to be equal to {0}, but instead was given {1}.", expected, actual);

            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_binary_content_and_check_content_type()
        {
            _controller.WithCallTo(c => c.BinaryStream())
                .ShouldRenderFileStream(ControllerResultTestController.BinaryStreamContents, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_binary_content_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryStream()).ShouldRenderFileStream(ControllerResultTestController.BinaryFileContents, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.",
                    contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_binary_content_and_check_invalid_content_type()
        {
            var content = new byte[] { 1, 2 };
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryStream()).ShouldRenderFileStream(content, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content()
        {
            _controller.WithCallTo(c => c.TextualStream())
                .ShouldRenderFileStream(ControllerResultTestController.TextualFileContent);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_textual_content()
        {
            const string contents = "dummy contents";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualStream()).ShouldRenderFileStream(contents));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file contents to be \"{0}\", but instead was \"{1}\".", contents, ControllerResultTestController.TextualFileContent)));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_and_check_content_type()
        {
            _controller.WithCallTo(c => c.TextualStream())
                .ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualStream()).ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.",
                    contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_textual_content_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";
            const string content = "dummy contents";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualStream()).ShouldRenderFileStream(content, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_using_given_char_encoding()
        {
            var encoding = Encoding.BigEndianUnicode;

            _controller.WithCallTo(c => c.TextualStream(encoding))
                .ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, encoding: encoding);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_using_given_char_encoding_and_check_content_type()
        {
            var encoding = Encoding.BigEndianUnicode;

            _controller.WithCallTo(c => c.TextualStream(encoding))
                .ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType, encoding);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_using_invalid_given_char_encoding_and_check_content_type()
        {
            Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualStream()).ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, encoding: Encoding.BigEndianUnicode));
        }

        [Test]
        public void Check_for_file_path_result()
        {
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath();
        }

        [Test]
        public void Check_for_file_path_result_and_check_file_name()
        {
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(ControllerResultTestController.FileName);
        }

        [Test]
        public void Check_for_file_path_result_and_check_invalid_file_name()
        {
            const string name = "dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(name));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file name to be '{0}', but instead was given '{1}'.", name, ControllerResultTestController.FileName)));
        }

        [Test]
        public void Check_for_file_path_result_and_check_file_name_and_check_content_type()
        {
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(ControllerResultTestController.FileName, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_path_result_and_check_file_name_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(ControllerResultTestController.FileName, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_path_result_and_check_invalid_file_name_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";
            const string name = "dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(name, contentType));

            // Assert that the content type validation occurs before that of the file name.
            Assert.That(exception.Message.Contains("content type"));
        }
    }
}