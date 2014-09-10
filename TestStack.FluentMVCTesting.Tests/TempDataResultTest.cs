using System.IO;
using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    [TestFixture]
    public class TempDataResultTestShould
    {
        private ControllerExtensionsController _controller;
        private TempDataResultTest _tempDataTest;

        [SetUp]
        public void Setup()
        {
            _controller = new ControllerExtensionsController();
            _tempDataTest = new TempDataResultTest(_controller);
        }

        [Test]
        public void Check_for_existent_temp_data_property()
        {
            const string key = "";
            _controller.TempData[key] = "";

            _tempDataTest.AndShouldHaveTempDataProperty(key);
        }

        [Test]
        public void Check_for_non_existent_temp_data_property()
        {
            const string key = "";

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _tempDataTest.AndShouldHaveTempDataProperty(key));

            Assert.That(exception.Message, Is.EqualTo(string.Format(
                "Expected TempData to have a non-null value with key \"{0}\", but none found.", key)));
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_value()
        {
            const string key = "";
            const int value = 10;
            _controller.TempData[key] = value;

            _tempDataTest.AndShouldHaveTempDataProperty(key, value);
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_invalid_value()
        {
            const string key = "";
            const int actualValue = 0;
            const int expectedValue = 1;
            _controller.TempData[key] = actualValue;

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _tempDataTest.AndShouldHaveTempDataProperty(key, expectedValue));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected value for key \"{0}\" to be \"{1}\", but instead found \"{2}\"", key, expectedValue, actualValue)));
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_invalid_value_of_different_types()
        {
            const string key = "";
            const int actualValue = 0;
            const string expectedValue = "one";
            _controller.TempData[key] = actualValue;

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _tempDataTest.AndShouldHaveTempDataProperty(key, expectedValue));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected value to be of type {0}, but instead was {1}.", expectedValue.GetType().FullName, actualValue.GetType().FullName)));
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_value_valid_using_referential_equality()
        {
            const string key = "";
            MemoryStream expectedValue = new MemoryStream();
            _controller.TempData[key] = expectedValue;

            _tempDataTest.AndShouldHaveTempDataProperty(key, expectedValue);
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_value_using_valid_predicate()
        {
            const string key = "";
            const int value = 1;
            _controller.TempData[key] = value;

            _tempDataTest.AndShouldHaveTempDataProperty<int>(key, x => x == value);
        }

        [Test]
        public void Check_for_existent_temp_data_property_and_check_value_using_invalid_predicate()
        {
            const string key = "";
            _controller.TempData[key] = 1;

            var exception = Assert.Throws<TempDataAssertionException>(() =>
                _tempDataTest.AndShouldHaveTempDataProperty<int>(key, x => x == 0));

            Assert.That(exception.Message, Is.EqualTo("Expected view model to pass the given condition, but it failed."));
        }
    }
}