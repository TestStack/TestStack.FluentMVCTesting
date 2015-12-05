using System;
using System.Web;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    internal class ActionRequestWrapper : HttpWorkerRequest
    {
        private readonly SimpleHttpRequest _httpRequest;

        public ActionRequestWrapper(SimpleHttpRequest httpRequest)
        {
            this._httpRequest = httpRequest;
        }

        public override string GetUriPath()
        {
            var path = this._httpRequest.UriPath;
            if (path.StartsWith("~"))
                path = path.Substring(1);
            return path;
        }

        public override string GetQueryString()
        {
            return String.Empty;
        }

        public override string GetRawUrl()
        {
            return this.GetUriPath();
        }

        public override string GetHttpVerbName()
        {
            return this._httpRequest.HttpMethod;
        }

        public override string GetHttpVersion()
        {
            return "HTTP/1.1";
        }

        public override string GetRemoteAddress()
        {
            return "localhost";
        }

        public override int GetRemotePort()
        {
            throw new NotImplementedException();
        }

        public override string GetLocalAddress()
        {
            return "127.0.0.1";
        }

        public override int GetLocalPort()
        {
            return 80;
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
            throw new NotImplementedException();
        }

        public override void SendKnownResponseHeader(int index, string value)
        {
            throw new NotImplementedException();
        }

        public override void SendUnknownResponseHeader(string name, string value)
        {
            throw new NotImplementedException();
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            throw new NotImplementedException();
        }

        public override void SendResponseFromFile(string filename, long offset, long length)
        {
            throw new NotImplementedException();
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length)
        {
            throw new NotImplementedException();
        }

        public override void FlushResponse(bool finalFlush)
        {
            throw new NotImplementedException();
        }

        public override void EndOfRequest()
        {
            throw new NotImplementedException();
        }
    }
}
