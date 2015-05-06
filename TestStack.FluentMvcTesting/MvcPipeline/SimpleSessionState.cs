using System.Collections.Generic;
using System.Web;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    internal class SimpleSessionState : HttpSessionStateBase
    {
        private readonly IDictionary<string, object> _values;

        public SimpleSessionState()
        {
            this._values = new Dictionary<string, object>();
        }

        public override void Add(string name, object value)
        {
            this._values.Add(name, value);
        }

        public override object this[string name]
        {
            get { return this._values[name]; }
            set { this._values[name] = value; }
        }
    }
}