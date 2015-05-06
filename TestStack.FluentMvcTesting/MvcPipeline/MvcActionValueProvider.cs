using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    public class MvcActionValueProvider : IValueProvider
    {
        private readonly Dictionary<string, object> _values;

        public MvcActionValueProvider(Expression body)
        {
            _values = new Dictionary<String, object>();

            var methodCallExpression = (MethodCallExpression)body;
            var methodParameters = methodCallExpression.Method.GetParameters();
            for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
            {
                var arg = methodCallExpression.Arguments[i];
                var par = methodParameters[i];
                _values.Add(par.Name, Invoke(arg));
            }
        }
        private static object Invoke(Expression valueExpression)
        {
            var convertExpression = Expression.Convert(valueExpression, typeof(object));
            var express = Expression.Lambda<Func<object>>(convertExpression).Compile();
            return express.Invoke();
        }
        public bool ContainsPrefix(string prefix)
        {
            return _values.ContainsKey(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            return new ValueProviderResult(_values[key], null, CultureInfo.InvariantCulture);
        }
    }
}
