using System;
using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    [TestFixture]
    class ControllerExtensionsShould
    {
        private ControllerExtensionsController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new ControllerExtensionsController();
        }

        [Test]
        public void Give_controller_modelstate_errors()
        {
            _controller.WithModelErrors();
            Assert.That(_controller.ModelState.IsValid, Is.False);
        }

        [Test]
        public void Call_action()
        {
            _controller.WithCallTo(c => c.SomeAction());
            Assert.That(_controller.SomeActionCalled);
        }

        [Test]
        public void Call_child_action()
        {
            _controller.WithCallToChild(c => c.SomeChildAction());
            Assert.That(_controller.SomeChildActionCalled);
        }

        [Test]
        public void Throw_exception_for_child_action_call_to_non_child_action()
        {
            var exception = Assert.Throws<InvalidControllerActionException>(() => _controller.WithCallToChild(c => c.SomeAction()));
            Assert.That(exception.Message, Is.EqualTo("Expected action SomeAction of controller ControllerExtensionsController to be a child action, but it didn't have the ChildActionOnly attribute."));
        }

        [Test]
        public void Check_for_existent_temp_data_property()
        {
            const string key = "";
            _controller.TempData[key] = "";

            _controller.ShouldHaveTempDataProperty(key);
        }

        [Test]
        public void Check_for_non_existent_temp_data_property()
        {
            const string key = "";
            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _controller.ShouldHaveTempDataProperty(key));

            Assert.That(exception.Message, Is.EqualTo(string.Format(
                "Expected TempData to have a non-null value with key \"{0}\", but none found.", key)));
        }
    }
}
