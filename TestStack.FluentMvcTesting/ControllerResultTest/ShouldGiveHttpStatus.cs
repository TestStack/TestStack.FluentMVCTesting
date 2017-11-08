using System.Net;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        public HttpStatusCodeResult ShouldGiveHttpStatus()
        {
            ValidateActionReturnType<HttpStatusCodeResult>();
            return (HttpStatusCodeResult)ActionResult;
        }

        public HttpStatusCodeResult ShouldGiveHttpStatus(int status)
        {
            ValidateActionReturnType<HttpStatusCodeResult>();

            var statusCodeResult = (HttpStatusCodeResult)ActionResult;

            if (statusCodeResult.StatusCode != status)
                throw new ActionResultAssertionException($"Expected HTTP status code to be '{status}', but instead received a '{statusCodeResult.StatusCode}'.");
            return (HttpStatusCodeResult)ActionResult;
        }

        public HttpStatusCodeResult ShouldGiveHttpStatus(HttpStatusCode status)
        {
            ShouldGiveHttpStatus((int)status);
            return (HttpStatusCodeResult)ActionResult;
        }
    }
}