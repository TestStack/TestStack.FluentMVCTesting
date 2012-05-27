using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using NUnit.Framework;
using TerseControllerTesting.Tests.TestControllers;

namespace TerseControllerTesting.Tests
{
    [TestFixture]
    class ControllerResultTestShould
    {
        private ControllerResultTestController _controller;
        #pragma warning disable 169
        private static readonly List<Tuple<string, TestAction>> ReturnTypes = new List<Tuple<string, TestAction>>
        {
            ReturnType<EmptyResult>(t => t.ShouldReturnEmptyResult()),
            ReturnType<RedirectResult>(t => t.ShouldRedirectTo("")),
            ReturnType<RedirectToRouteResult>(t => t.ShouldRedirectTo(c => c.EmptyResult)),
        };
        private static readonly List<Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>>> ActionRedirects = new List<Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>>>
        {
            ActionRedirect("ActionWithNoParameters",
                t => t.ShouldRedirectTo(c => c.ActionWithNoParameters),
                c => c.RedirectToActionWithNoParameters()
            ),
            ActionRedirect("ActionWithOneParameter",
                t => t.ShouldRedirectTo(c => c.ActionWithOneParameter),
                c => c.RedirectToActionWithOneParameter()
            ),
            ActionRedirect("ActionWithOneParameter",
                t => t.ShouldRedirectTo<int>(c => c.ActionWithOneParameter),
                c => c.RedirectToActionWithOneParameter()
            ),
            ActionRedirect("ActionWithTwoParameters",
                t => t.ShouldRedirectTo<int, int>(c => c.ActionWithTwoParameters),
                c => c.RedirectToActionWithTwoParameters()
            ),
            ActionRedirect("ActionWithThreeParameters",
                t => t.ShouldRedirectTo<int, int, int>(c => c.ActionWithThreeParameters),
                c => c.RedirectToActionWithThreeParameters()
            ),
            ActionRedirect("ActionWithOneParameter",
                t => t.ShouldRedirectTo(c => c.ActionWithOneParameter(0)),
                c => c.RedirectToActionWithOneParameter()
            ),
            ActionRedirect("ActionWithTwoParameters",
                t => t.ShouldRedirectTo(c => c.ActionWithTwoParameters(0, 0)),
                c => c.RedirectToActionWithTwoParameters()
            ),
            ActionRedirect("ActionWithThreeParameters",
                t => t.ShouldRedirectTo(c => c.ActionWithThreeParameters(0, 0, 0)),
                c => c.RedirectToActionWithThreeParameters()
            ),
            ActionRedirect("ActionWithMoreThanThreeParameters",
                t => t.ShouldRedirectTo(c => c.ActionWithMoreThanThreeParameters(0, 0, 0, 0)),
                c => c.RedirectToActionWithMoreThanThreeParameters()
            ),
        };
        #pragma warning restore 169
        
        public delegate void TestAction(ControllerResultTest<ControllerResultTestController> testClass);
        private static Tuple<string, TestAction> ReturnType<T>(TestAction a)
        {
            return new Tuple<string, TestAction>(typeof(T).Name, a);
        }
        private static Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>> ActionRedirect(string s, TestAction a, Expression<Func<ControllerResultTestController, ActionResult>> c)
        {
            return new Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>>(s, a, c);
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
            _controller.WithCallTo(c => c.RedirectToUrl()).ShouldRedirectTo(ControllerResultTestController.RedirectUrl);
        }

        [Test]
        public void Check_for_redirect_to_invalid_url()
        {
            const string url = "http://validurl/";
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RedirectToUrl()).ShouldRedirectTo(url)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected redirect to URL '{0}', but instead was given a redirect to URL '{1}'.", url, ControllerResultTestController.RedirectUrl)));
        }

        [Test]
        public void Check_for_redirect_to_route_name()
        {
            _controller.WithCallTo(c => c.RedirectToRouteName()).ShouldRedirectToRoute(ControllerResultTestController.RouteName);
        }

        [Test]
        public void Check_for_redirect_to_invalid_route_name()
        {
            const string routeName = "ValidRoute";
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RedirectToRouteName()).ShouldRedirectToRoute(routeName)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected redirect to route '{0}', but instead was given a redirect to route '{1}'.", routeName, ControllerResultTestController.RouteName)));
        }

        [Test]
        [TestCaseSource("ActionRedirects")]
        public void Check_for_redirect_to_action(Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>> test)
        {
            test.Item2(_controller.WithCallTo(test.Item3));
        }

        [Test]
        [TestCaseSource("ActionRedirects")]
        public void Check_for_redirect_to_incorrect_controller(Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>> test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.RedirectToAnotherController()))
            );
            Assert.That(exception.Message, Is.EqualTo("Expected redirect to controller 'ControllerResultTest', but instead was given a redirect to controller 'SomeOtherController'."));
        }

        [Test]
        [TestCaseSource("ActionRedirects")]
        public void Check_for_redirect_to_incorrect_action(Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>> test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.RedirectToRandomResult()))
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected redirect to action '{0}', but instead was given a redirect to action 'RandomResult'.", test.Item1)));
        }

        [Test]
        [TestCaseSource("ActionRedirects")]
        public void Check_for_redirect_to_empty_action(Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>> test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.RedirectToRouteName()))
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected redirect to action '{0}', but instead was given a redirect without an action.", test.Item1)));
        }
    }
}
