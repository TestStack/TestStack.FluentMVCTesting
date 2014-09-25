using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    partial class ControllerResultTestShould
    {
        [Test]
        public void Check_for_default_view_or_partial()
        {
            _controller.WithCallTo(c => c.DefaultView()).ShouldRenderDefaultView();
            _controller.WithCallTo(c => c.DefaultView()).ShouldRenderView("DefaultView");
            _controller.WithCallTo(c => c.DefaultViewExplicit()).ShouldRenderDefaultView();
            _controller.WithCallTo(c => c.DefaultPartial()).ShouldRenderDefaultPartialView();
            _controller.WithCallTo(c => c.DefaultPartial()).ShouldRenderPartialView("DefaultPartial");
            _controller.WithCallTo(c => c.DefaultPartialExplicit()).ShouldRenderDefaultPartialView();
        }

        [Test]
        public void Check_for_invalid_view_name_when_expecting_default_view()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RandomView()).ShouldRenderDefaultView()
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected result view to be 'RandomView', but instead was given '{0}'.", ControllerResultTestController.RandomViewName)));
        }

        [Test]
        public void Check_for_invalid_view_name_when_expecting_default_partial()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RandomPartial()).ShouldRenderDefaultPartialView()
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected result view to be 'RandomPartial', but instead was given '{0}'.", ControllerResultTestController.RandomViewName)));
        }

        [Test]
        public void Check_for_named_view_or_partial()
        {
            _controller.WithCallTo(c => c.NamedView()).ShouldRenderView(ControllerResultTestController.ViewName);
            _controller.WithCallTo(c => c.NamedPartial()).ShouldRenderPartialView(ControllerResultTestController.PartialName);
        }

        [Test]
        public void Check_for_invalid_view_name()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RandomView()).ShouldRenderView(ControllerResultTestController.ViewName)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected result view to be '{0}', but instead was given '{1}'.", ControllerResultTestController.ViewName, ControllerResultTestController.RandomViewName)));
        }

        [Test]
        public void Check_for_invalid_partial_name()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RandomPartial()).ShouldRenderPartialView(ControllerResultTestController.PartialName)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected result view to be '{0}', but instead was given '{1}'.", ControllerResultTestController.PartialName, ControllerResultTestController.RandomViewName)));
        }
    }
}