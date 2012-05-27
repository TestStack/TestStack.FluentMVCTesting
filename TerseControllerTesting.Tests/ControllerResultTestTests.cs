using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;
using TerseControllerTesting.Tests.TestControllers;

namespace TerseControllerTesting.Tests
{
    [TestFixture]
    class ControllerResultTestShould
    {
        private ControllerResultTestController _controller;
        private static readonly List<Tuple<string, TestAction>> ReturnTypes = new List<Tuple<string, TestAction>>
        {
            ReturnType<EmptyResult>(t => t.ShouldReturnEmptyResult()),
            ReturnType<RedirectResult>(t => t.ShouldRedirectTo("")),
        };

        public delegate void TestAction(ControllerResultTest<ControllerResultTestController> testClass);
        private static Tuple<string, TestAction> ReturnType<T>(TestAction a)
        {
            return new Tuple<string, TestAction>(typeof(T).Name, a);
        }

        [SetUp]
        public void Setup()
        {
            _controller = new ControllerResultTestController();
        }

        [Test]
        [TestCaseSource("ReturnTypes")]
        public void Check_return_type(Tuple<string, TestAction> test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.RandomResult()))
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected action result to be a {0}, but instead received a RandomResult.", test.Item1)));
        }

        [Test]
        [TestCaseSource("ReturnTypes")]
        public void Check_null_return(Tuple<string, TestAction> test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.Null()))
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Received null action result when expecting {0}.", test.Item1)));
        }

        [Test]
        public void Check_for_empty_result()
        {
            _controller.WithCallTo(c => c.EmptyResult()).ShouldReturnEmptyResult();
        }

        [Test]
        public void Check_for_redirect_to_url()
        {
            _controller.WithCallTo(c => c.RedirectToUrl()).ShouldRedirectTo("http://url/");
        }

        [Test]
        public void Check_for_redirect_to_invalid_url()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RedirectToUrl()).ShouldRedirectTo("http://validurl/")
            );
            Assert.That(exception.Message, Is.EqualTo("Expected redirect to URL 'http://validurl/', but instead was given a redirect to URL 'http://url/'."));
        }
    }
}
