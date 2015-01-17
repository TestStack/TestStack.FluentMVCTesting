using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    partial class ControllerResultTestShould
    {
        // Different ways that action redirects can be asserted along with the expected method name and the correct controller action call for that assertion
        private static readonly List<RedirectToActionTestMetadata> ActionRedirects = new List<RedirectToActionTestMetadata>
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
            ActionRedirect("ActionWithNoParameters",
                t => t.ShouldRedirectTo(c => c.ActionWithNoParameters()),
                c => c.RedirectToActionWithNoParameters()
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
            ActionRedirect("ActionWithMoreThanThreeParameters",
                t => t.ShouldRedirectTo(typeof(ControllerResultTestController).GetMethod("ActionWithMoreThanThreeParameters")),
                c => c.RedirectToActionWithMoreThanThreeParameters()
            ),
        };

        // Different ways that redirects to another controller can be asserted
        private static readonly List<TestAction> OtherControllerRedirects = new List<TestAction>
        {
            c => c.ShouldRedirectTo<SomeOtherController>(typeof(SomeOtherController).GetMethod("SomeAction")),
            c => c.ShouldRedirectTo<SomeOtherController>(c2 => c2.SomeAction()),
        };

        public class RedirectToActionTestMetadata : Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>>
        {
            public RedirectToActionTestMetadata(string expectedMethodName, TestAction testCall, Expression<Func<ControllerResultTestController, ActionResult>> validControllerActionCall)
                : base(expectedMethodName, testCall, validControllerActionCall) { }
        }

        public delegate void TestAction(ControllerResultTest<ControllerResultTestController> testClass);

        private static Tuple<string, TestAction> ReturnType<T>(TestAction a)
        {
            return new Tuple<string, TestAction>(typeof(T).Name, a);
        }

        private static RedirectToActionTestMetadata ActionRedirect(string s, TestAction a, Expression<Func<ControllerResultTestController, ActionResult>> c)
        {
            return new RedirectToActionTestMetadata(s, a, c);
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
        public void Check_for_redirect_to_action(RedirectToActionTestMetadata test)
        {
            test.Item2(_controller.WithCallTo(test.Item3));
        }

        [Test]
        [TestCaseSource("ActionRedirects")]
        public void Check_for_redirect_to_incorrect_controller(RedirectToActionTestMetadata test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.RedirectToAnotherController()))
            );
            Assert.That(exception.Message, Is.EqualTo("Expected redirect to controller 'ControllerResultTest', but instead was given a redirect to controller 'SomeOther'."));
        }

        [Test]
        [TestCaseSource("ActionRedirects")]
        public void Check_for_redirect_to_incorrect_action(RedirectToActionTestMetadata test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.RedirectToRandomResult()))
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected redirect to action '{0}', but instead was given a redirect to action 'RandomResult'.", test.Item1)));
        }

        // todo: Test the route values expectations

        [Test]
        [TestCaseSource("ActionRedirects")]
        public void Check_for_redirect_to_empty_action(RedirectToActionTestMetadata test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.RedirectToRouteName()))
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected redirect to action '{0}', but instead was given a redirect without an action.", test.Item1)));
        }

        [Test]
        [TestCaseSource("OtherControllerRedirects")]
        public void Check_for_redirect_to_another_controller(TestAction action)
        {
            action(_controller.WithCallTo(c => c.RedirectToAnotherController()));
        }

        [Test]
        public void Check_for_redirect_to_incorrect_other_controller()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RedirectToAnotherController()).ShouldRedirectTo<YetAnotherController>(c => c.SomeAction())
            );
            Assert.That(exception.Message, Is.EqualTo("Expected redirect to controller 'YetAnother', but instead was given a redirect to controller 'SomeOther'."));
        }

        [Test]
        public void Check_for_redirect_to_incorrect_action_in_another_controller()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RedirectToAnotherController()).ShouldRedirectTo<SomeOtherController>(c => c.SomeOtherAction())
            );
            Assert.That(exception.Message, Is.EqualTo("Expected redirect to action 'SomeOtherAction', but instead was given a redirect to action 'SomeAction'."));
        }

        [Test]
        public void Check_for_redirect_to_action_within_same_controller()
        {
            _controller.WithCallTo(c => c.RedirectToActionWithNoParameters()).ShouldRedirectTo<ControllerResultTestController>(c => c.ActionWithNoParameters());
        }

        [Test]
        public void Check_for_redirect_to_action_within_different_controller()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RedirectToActionWithNoParameters()).ShouldRedirectTo<SomeOtherController>(c => c.SomeAction()));
            Console.WriteLine(exception);
            Assert.That(exception.Message, Is.EqualTo("Expected redirect to action 'SomeAction' in 'SomeOther' controller, but instead was given redirect to action 'ActionWithNoParameters' within the same controller."));
        }
    }
}