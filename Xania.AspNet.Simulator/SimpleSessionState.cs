using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.SessionState;

namespace Xania.AspNet.Simulator
{
    internal class SimpleSessionState : HttpSessionStateBase
    {
        private readonly IDictionary<string, object> _values;

        public SimpleSessionState()
        {
            _values = new Dictionary<string, object>();
        }

        public override void Add(string name, object value)
        {
            _values.Add(name, value);
        }

        public override object this[string name]
        {
            get { return _values[name]; }
            set { _values[name] = value; }
        }
    }
}