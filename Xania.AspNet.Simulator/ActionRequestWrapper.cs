using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Xania.AspNet.Simulator
{
    public class ActionRequestWrapper : HttpWorkerRequest
    {
        private readonly IHttpRequest _httpRequest;

        public ActionRequestWrapper(IHttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public override string GetUriPath()
        {
            var path = _httpRequest.UriPath;
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
            return GetUriPath();
        }

        public override string GetHttpVerbName()
        {
            return _httpRequest.HttpMethod;
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

    public interface IHttpRequest
    {
        string UriPath { get; }
        string HttpMethod { get; }
        IPrincipal User { get; }
    }
}
