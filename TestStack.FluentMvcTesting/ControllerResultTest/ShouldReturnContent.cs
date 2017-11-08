using System.Text;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        public ContentResult ShouldReturnContent(string content = null, string contentType = null, Encoding encoding = null)
        {
            ValidateActionReturnType<ContentResult>();
            var contentResult = (ContentResult)ActionResult;

            if (contentType != null && contentType != contentResult.ContentType)
                throw new ActionResultAssertionException($"Expected content type to be '{contentType}', but instead was '{contentResult.ContentType}'.");

            if (content != null && content != contentResult.Content)
                throw new ActionResultAssertionException($"Expected content to be '{content}', but instead was '{contentResult.Content}'.");

            if (encoding != null && encoding != contentResult.ContentEncoding)
            {
                var actualEncoding = contentResult.ContentEncoding?.EncodingName ?? "null";
                throw new ActionResultAssertionException($"Expected encoding to be equal to {encoding.EncodingName}, but instead was {actualEncoding}.");
            }

            return contentResult;
        }
    }
}