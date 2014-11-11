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
            {
                throw new ActionResultAssertionException(string.Format(
                    "Expected content type to be \"{0}\", but instead was \"{1}\".",
                    contentType,
                    contentResult.ContentType));
            }

            if (content != null && content != contentResult.Content)
            {
                throw new ActionResultAssertionException(string.Format(
                    "Expected content to be \"{0}\", but instead was \"{1}\".",
                    content,
                    contentResult.Content));
            }

            if (encoding != null && encoding != contentResult.ContentEncoding)
            {
                throw new ActionResultAssertionException(string.Format(
                    "Expected encoding to be equal to {0}, but instead was {1}.",
                    encoding.EncodingName,
                    contentResult.ContentEncoding != null ? contentResult.ContentEncoding.EncodingName : "null"));
            }

            return contentResult;
        }
    }
}