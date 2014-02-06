using System.Collections.Generic;
using System.Web.Routing;

namespace TestStack.FluentMVCTesting
{
    public static class RouteValueDictionaryExtension
    {
        public static RouteValueDictionary WithRouteValue(this RouteValueDictionary dict, string key)
        {
            if (!dict.ContainsKey(key))
            {
                throw new MissingRouteValueException(string.Format("No value {0} in the route dictionary", key));
            }

            return dict;
        }

        public static RouteValueDictionary WithRouteValue<T>(this RouteValueDictionary dict, string key, T value)
        {
            dict.WithRouteValue(key);

            var actualValue = dict[key];

            if (!(actualValue is T))
            {
                throw new ValueTypeMismatchException(string.Format("Invalid type of Value with key {0} \r\n expected {1} \r\n but got {2}", key, value.GetType(), actualValue.GetType()));
            }

            if (!Equals(actualValue, value))
            {
                throw new InvalidRouteValueException(string.Format("Invalid value for key {0} \r\n expected {1} \r\n but got {2} in the route dictionary",key ,value, actualValue));
            }

            return dict;
        }
    }
}
