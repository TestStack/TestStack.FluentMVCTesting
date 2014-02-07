using System.Web.Routing;
using NUnit.Framework;

namespace TestStack.FluentMVCTesting.Tests
{
    [TestFixture]
    class RouteValueDictionaryExtensionsTests
    {
        [Test]
        public void WithRouteValue_ThrowsWhenDictIsEmpty()
        {
            var dict = new RouteValueDictionary();

            Assert.Throws<MissingRouteValueException>(() => dict.WithRouteValue("NotImportant"));
        }

        [Test]
        public void WithRouteValue_ThrowsWhenDictDoesntHaveValue()
        {
            var dict = new RouteValueDictionary();
            dict.Add("Existing", "NotImportant");

            Assert.Throws<MissingRouteValueException>(() => dict.WithRouteValue("NotExisting"));
        }

        [Test]
        public void WithRouteValue_ThrowsWhenDictHasIncorrectValue()
        {
            var dict = new RouteValueDictionary();
            dict.Add("Existing", "Correct");

            Assert.Throws<InvalidRouteValueException>(() => dict.WithRouteValue("Existing", "Incorrect"));
        }

        [Test]
        public void WithRouteValue_ThrowIfValueTypeMismatch()
        {
            var dict = new RouteValueDictionary();
            dict.Add("Existing", 1);

            Assert.Throws<ValueTypeMismatchException>(() => dict.WithRouteValue("Existing", "1"));
        }

        [Test]
        public void WithRouteValue_DoesntThrowIfValueIsCorrect()
        {
            var dict = new RouteValueDictionary();
            dict.Add("Existing", "Correct");

            dict.WithRouteValue("Existing", "Correct");

            Assert.Pass();
        }

         [Test]
        public void WithRouteValue_CanUseIntType()
        {
            var dict = new RouteValueDictionary();
            dict.Add("Existing", 10);

            dict.WithRouteValue("Existing", 10);
            
             Assert.Pass();
        }
   }
}
