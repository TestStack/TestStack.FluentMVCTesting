using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    class AsyncControllersShould
    {
        private AsyncController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new AsyncController();
        }

        [Test]
        public void Work_correctly_for_valid_check()
        {
            _controller.WithCallTo(c => c.AsyncViewAction())
                .ShouldRenderDefaultView();
        }

        [Test]
        public void Work_correctly_for_invalid_check()
        {
            Assert.Throws<ActionResultAssertionException>(
                () => _controller.WithCallTo(c => c.AsyncViewAction()).ShouldGiveHttpStatus()
            );
        }
        [Test]
        public void Work_correctly_for_valid_child_action_check()
        {
            _controller.WithCallToChild(c => c.AsyncChildViewAction())
                .ShouldRenderDefaultView();
        }

        [Test]
        public void Work_correctly_for_invalid_child_action_check()
        {
            Assert.Throws<InvalidControllerActionException>(
                () => _controller.WithCallToChild(c => c.AsyncViewAction())
            );
        }
    }
}
