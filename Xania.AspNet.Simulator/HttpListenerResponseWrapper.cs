using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Xania.AspNet.Simulator
{
    internal class HttpListenerResponseWrapper : HttpResponseBase, IDisposable
    {
        private readonly HttpListenerResponse _listenerResponse;
        private TextWriter _output;
        private readonly MemoryStream _outputStream;
        private bool _closed = false;

        public HttpListenerResponseWrapper(HttpListenerResponse listenerResponse)
        {
            _listenerResponse = listenerResponse;
            _outputStream = new MemoryStream();
            _output = new StreamWriter(_outputStream);
        }

        public override string ContentType { get; set; }

        public override Encoding ContentEncoding { get; set; }

        public override int StatusCode
        {
            get { return _listenerResponse.StatusCode; }
            set { _listenerResponse.StatusCode = value; }
        }

        public override string Status
        {
            get 
            {
                return this.StatusCode.ToString(NumberFormatInfo.InvariantInfo) + " " + this.StatusDescription;
            }
            set
            {
                int i = value.IndexOf(' ');
                StatusCode = Int32.Parse(value.Substring(0, i), CultureInfo.InvariantCulture);
                StatusDescription = value.Substring(i + 1);
            }
        }

        public override string StatusDescription
        {
            get { return _listenerResponse.StatusDescription; }
            set { _listenerResponse.StatusDescription = value; }
        }

        public override TextWriter Output
        {
            get { return _output; }
            set { _output = value; }
        }

        public override Stream OutputStream
        {
            get { return _outputStream; }
        }

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }

        public override void Flush()
        {
        }

        public override void Write(char ch)
        {
            Output.Write(ch);
        }

        public override void Write(string s)
        {
            Output.Write(s);
        }

        public override void Close()
        {
            if (!_closed)
            {
                _closed = true;
                _output.Flush();

                var buffer = _outputStream.ToArray();
                _listenerResponse.ContentLength64 = buffer.Length;
                var output = _listenerResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

        public void Dispose()
        {
            if (_outputStream != null)
            {
                _outputStream.Dispose();
            }
        }
    }
}