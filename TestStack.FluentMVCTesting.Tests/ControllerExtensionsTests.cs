using System.IO;
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
        public void Check_for_unexpected_non_existent_temp_data_property()
        {
            const string key = "";

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _controller.ShouldHaveTempDataProperty(key));

            Assert.That(exception.Message, Is.EqualTo(string.Format(
                "Expected TempData to have a non-null value with key '{0}', but none found.", key)));
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_value()
        {
            const string key = "";
            const int value = 10;
            _controller.TempData[key] = value;

            _controller.ShouldHaveTempDataProperty(key, value);
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_invalid_value()
        {
            const string key = "";
            const int actualValue = 0;
            const int expectedValue = 1;
            _controller.TempData[key] = actualValue;

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _controller.ShouldHaveTempDataProperty(key, expectedValue));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected value for key '{0}' to be '{1}', but instead found '{2}'", key, expectedValue, actualValue)));
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_invalid_value_of_different_types()
        {
            const string key = "";
            const int actualValue = 0;
            const string expectedValue = "one";
            _controller.TempData[key] = actualValue;

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _controller.ShouldHaveTempDataProperty(key, expectedValue));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected value to be of type {0}, but instead was {1}.", expectedValue.GetType().FullName, actualValue.GetType().FullName)));
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_value_valid_using_referential_equality()
        {
            const string key = "";
            MemoryStream expectedValue = new MemoryStream();
            _controller.TempData[key] = expectedValue;

            _controller.ShouldHaveTempDataProperty(key, expectedValue);
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_value_using_valid_predicate()
        {
            const string key = "";
            const int value = 1;
            _controller.TempData[key] = value;

            _controller
                .ShouldHaveTempDataProperty<int>(key, x => x == value);
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_value_using_invalid_predicate()
        {
            const string key = "";
            _controller.TempData[key] = 1;

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _controller.ShouldHaveTempDataProperty<int>(key, x => x == 0));

            Assert.That(exception.Message, Is.EqualTo("Expected view model to pass the given condition, but it failed."));
        }

        [Test]
        public void Check_for_unexpected_non_existent_temp_data_property_when_supplied_with_predicate()
        {
            const string key = "";

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _controller.ShouldHaveTempDataProperty<int>(key, x => x == 0));

            Assert.That(exception.Message, Is.EqualTo(string.Format(
                "Expected TempData to have a non-null value with key '{0}', but none found.", key)));
        }

        [Test]
        public void Check_for_non_existent_temp_data_property()
        {
            _controller
                .ShouldNotHaveTempDataProperty("");
        }

        [Test]
        public void Check_for_unexpected_existent_temp_data_property()
        {
            const string Key = "";
            _controller.TempData[Key] = "";

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _controller.ShouldNotHaveTempDataProperty(Key));

            Assert.That(exception.Message, Is.EqualTo(string.Format(
                "Expected TempData to have no value with key '{0}', but found one.", Key)));
        }
    }
}
