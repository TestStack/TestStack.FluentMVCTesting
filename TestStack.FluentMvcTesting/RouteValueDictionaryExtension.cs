using System.Web.Routing;

namespace TestStack.FluentMVCTesting
{
    public static class RouteValueDictionaryExtension
    {
        public static RouteValueDictionary WithRouteValue(this RouteValueDictionary dict, string key)
        {
            if (!dict.ContainsKey(key))
                throw new MissingRouteValueException($"No value {key} in the route dictionary");

            return dict;
        }

        public static RouteValueDictionary WithRouteValue<T>(this RouteValueDictionary dict, string key, T value)
        {
            dict.WithRouteValue(key);

            var actualValue = dict[key];

            if (!(actualValue is T))
                throw new ValueTypeMismatchException($"Invalid type of Value with key {key} \r\n expected {value.GetType()} \r\n but got {actualValue.GetType()}");

            if (!Equals(actualValue, value))
                throw new InvalidRouteValueException($"Invalid value for key {key} \r\n expected {value} \r\n but got {actualValue} in the route dictionary");

            return dict;
        }
    }
}
