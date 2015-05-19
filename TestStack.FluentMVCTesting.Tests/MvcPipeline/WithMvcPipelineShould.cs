namespace TestStack.FluentMVCTesting.Tests.MvcPipeline
{
    using NUnit.Framework;
    using TestStack.FluentMVCTesting.Tests.TestControllers;

#if NET45
    [TestFixture]
    class WithMvcPipelineShould
    {
        [Test]
        public void Call_action()
        {
            var controller = new ControllerExtensionsController();
            controller.WithMvcPipelineTo(c => c.SomeAction());
            Assert.That(controller.SomeActionCalled);
        }

        [Test]
        public void Work_correctly_for_valid_check()
        {
            new AsyncController()
                .WithMvcPipelineTo(c => c.AsyncViewAction())
                .ShouldRenderDefaultView();
        }

        [Test]
        public void Work_correctly_for_invalid_check()
        {
            var controller = new AsyncController();
            Assert.Throws<ActionResultAssertionException>(
                () => controller.WithMvcPipelineTo(c => c.AsyncViewAction()).ShouldGiveHttpStatus()
            );
        }
    }
#endif
}
