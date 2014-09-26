using System.Net;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public partial class ControllerResultTest<T>
    {
        public void ShouldGiveHttpStatus()
        {
            ValidateActionReturnType<HttpStatusCodeResult>();
        }

        public void ShouldGiveHttpStatus(int status)
        {
            ValidateActionReturnType<HttpStatusCodeResult>();

            var statusCodeResult = (HttpStatusCodeResult)_actionResult;

            if (statusCodeResult.StatusCode != status)
                throw new ActionResultAssertionException(string.Format("Expected HTTP status code to be '{0}', but instead received a '{1}'.", status, statusCodeResult.StatusCode));
        }

        public void ShouldGiveHttpStatus(HttpStatusCode status)
        {
            ShouldGiveHttpStatus((int)status);
        }
    }
}