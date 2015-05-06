using System.Web;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    internal class HttpRequestSimulator : HttpRequestWrapper
    {
        public HttpRequestSimulator(HttpRequest httpRequest)
            : base(httpRequest)
        {
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get { return "~" + this.Url.AbsolutePath; }
        }
    }
}