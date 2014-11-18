using System.Net;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        public void ShouldGiveHttpStatus() => ValidateActionReturnType<HttpStatusCodeResult>();

        public void ShouldGiveHttpStatus(int status)
        {
            ValidateActionReturnType<HttpStatusCodeResult>();

            var statusCodeResult = (HttpStatusCodeResult)ActionResult;

            if (statusCodeResult.StatusCode != status)
                throw new ActionResultAssertionException("Expected HTTP status code to be '\{status}', but instead received a '\{statusCodeResult.StatusCode}'.");
        }

        public void ShouldGiveHttpStatus(HttpStatusCode status) => ShouldGiveHttpStatus((int)status);
    }
}