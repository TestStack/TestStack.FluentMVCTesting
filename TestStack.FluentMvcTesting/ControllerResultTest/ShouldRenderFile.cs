using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        private static void EnsureContentTypeIsSame(string actual, string expected)
        {
            if (expected == null) return;
            if (actual != expected)
                throw new ActionResultAssertionException($"Expected file to be of content type '{expected}', but instead was given '{actual}'.");
        }

        private static byte[] ConvertStreamToArray(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                stream.Position = 0;
                return memoryStream.ToArray();
            }
        }

        public FileResult ShouldRenderAnyFile(string contentType = null)
        {
            ValidateActionReturnType<FileResult>();
            var fileResult = (FileResult)ActionResult;

            EnsureContentTypeIsSame(fileResult.ContentType, contentType);

            return fileResult;
        }

        public FileContentResult ShouldRenderFileContents(byte[] contents = null, string contentType = null)
        {
            ValidateActionReturnType<FileContentResult>();
            var fileResult = (FileContentResult)ActionResult;

            EnsureContentTypeIsSame(fileResult.ContentType, contentType);

            if (contents != null && !fileResult.FileContents.SequenceEqual(contents))
                throw new ActionResultAssertionException($"Expected file contents to be equal to [{string.Join(", ", contents)}], but instead was given [{string.Join(", ", fileResult.FileContents)}].");

            return fileResult;
        }

        public FileContentResult ShouldRenderFileContents(string contents, string contentType = null, Encoding encoding = null)
        {
            ValidateActionReturnType<FileContentResult>();
            var fileResult = (FileContentResult)ActionResult;

            EnsureContentTypeIsSame(fileResult.ContentType, contentType);

            if (encoding == null)
                encoding = Encoding.UTF8;

            var reconstitutedText = encoding.GetString(fileResult.FileContents);
            if (contents != reconstitutedText)
                throw new ActionResultAssertionException($"Expected file contents to be '{contents}', but instead was '{reconstitutedText}'.");

            return fileResult;
        }

        public FileStreamResult ShouldRenderFileStream(byte[] content, string contentType = null)
        {
            var reconstitutedStream = new MemoryStream(content);
            return ShouldRenderFileStream(reconstitutedStream, contentType);
        }

        public FileStreamResult ShouldRenderFileStream(Stream stream = null, string contentType = null)
        {
            ValidateActionReturnType<FileStreamResult>();
            var fileResult = (FileStreamResult)ActionResult;

            EnsureContentTypeIsSame(fileResult.ContentType, contentType);

            if (stream != null)
            {
                byte[] expected = ConvertStreamToArray(stream);
                byte[] actual = ConvertStreamToArray(fileResult.FileStream);

                if (!expected.SequenceEqual(actual))
                {
                    throw new ActionResultAssertionException($"Expected file contents to be equal to [{string.Join(", ", expected)}], but instead was given [{string.Join(", ", actual)}].");
                }
            }

            return fileResult;
        }

        public FileStreamResult ShouldRenderFileStream(string contents, string contentType = null, Encoding encoding = null)
        {
            ValidateActionReturnType<FileStreamResult>();
            var fileResult = (FileStreamResult)ActionResult;

            EnsureContentTypeIsSame(fileResult.ContentType, contentType);

            if (encoding == null)
                encoding = Encoding.UTF8;

            var reconstitutedText = encoding.GetString(ConvertStreamToArray(fileResult.FileStream));
            if (contents != reconstitutedText)
                throw new ActionResultAssertionException($"Expected file contents to be '{contents}', but instead was '{reconstitutedText}'.");

            return fileResult;
        }

        public FilePathResult ShouldRenderFilePath(string fileName = null, string contentType = null)
        {
            ValidateActionReturnType<FilePathResult>();
            var fileResult = (FilePathResult)ActionResult;

            EnsureContentTypeIsSame(fileResult.ContentType, contentType);

            if (fileName != null && fileName != fileResult.FileName)
                throw new ActionResultAssertionException($"Expected file name to be '{fileName}', but instead was given '{fileResult.FileName}'.");

            return fileResult;
        }
    }
}