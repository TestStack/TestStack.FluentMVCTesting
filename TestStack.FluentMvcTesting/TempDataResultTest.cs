using System;
using System.Web.Mvc;

namespace TestStack.FluentMVCTesting
{
    public class TempDataResultTest
    {
        private readonly ControllerBase _controller;

        public TempDataResultTest(ControllerBase controller)
        {
            _controller = controller;
        }

        public void AndShouldHaveTempDataProperty(string key, object value = null)
        {
            var actual = _controller.TempData[key];

            if (actual == null)
            {
                throw new TempDataAssertionException(string.Format(
                    "Expected TempData to have a non-null value with key \"{0}\", but none found.", key));
            }

            if (value != null && actual.GetType() != value.GetType())
            {
                throw new TempDataAssertionException(string.Format(
                    "Expected value to be of type {0}, but instead was {1}.",
                    value.GetType().FullName,
                    actual.GetType().FullName));
            }

            if (value != null && !value.Equals(actual))
            {
                throw new TempDataAssertionException(string.Format(
                    "Expected value for key \"{0}\" to be \"{1}\", but instead found \"{2}\"", key, value, actual));
            }
        }

        public void AndShouldHaveTempDataProperty<TValue>(string key, Func<TValue, bool> predicate)
        {
            var actual = _controller.TempData[key];

            if (actual == null)
            {
                throw new TempDataAssertionException(string.Format(
                    "Expected TempData to have a non-null value with key \"{0}\", but none found.", key));
            }

            if (!predicate((TValue)actual))
            {
                throw new TempDataAssertionException("Expected view model to pass the given condition, but it failed.");
            }
        }
    }
}