using System.Net;
using System.Web;

namespace Xania.AspNet.Simulator
{
    internal class HttpContextSimulator : HttpContextWrapper
    {
        private readonly HttpRequestSimulator _request;
        private readonly HttpResponseWrapper _response;
        private readonly SimpleSessionState _session;

        public HttpContextSimulator(HttpContext httpContext) : base(httpContext)
        {
            _request = new HttpRequestSimulator(httpContext.Request);
            _response = new HttpResponseWrapper(httpContext.Response);
            _session = new SimpleSessionState();
        }

        public override HttpRequestBase Request
        {
            get { return _request; }
        }

        public override HttpResponseBase Response
        {
            get { return _response; }
        }

        public override HttpSessionStateBase Session
        {
            get { return _session; }
        }
    }

    public class HttpListenerContextSimulator : HttpContextBase
    {
        private readonly HttpListenerContext _listenerContext;
        private readonly HttpListenerResponseWrapper _response;
        private readonly HttpListenerRequestWrapper _request;

        public HttpListenerContextSimulator(HttpListenerContext listenerContext)
        {
            _listenerContext = listenerContext;
            _response = new HttpListenerResponseWrapper(listenerContext.Response);
            _request = new HttpListenerRequestWrapper(listenerContext.Request);
        }

        public override HttpRequestBase Request
        {
            get { return _request; }
        }

        public override HttpResponseBase Response
        {
            get { return _response; }
        }
    }
}