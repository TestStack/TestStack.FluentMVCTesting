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

        public TempDataResultTest AndShouldHaveTempDataProperty(string key, object value = null)
        {
            _controller.ShouldHaveTempDataProperty(key, value);
            return this;
        }

        public TempDataResultTest AndShouldHaveTempDataProperty<TValue>(string key, Func<TValue, bool> predicate)
        {
            _controller.ShouldHaveTempDataProperty(key, predicate);
            return this;
        }

        public TempDataResultTest AndShouldNotHaveTempDataProperty(string empty)
        {
            _controller.ShouldNotHaveTempDataProperty(empty);
            return this;
        }
    }
}