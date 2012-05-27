using NUnit.Framework;
using TerseControllerTesting.Tests.TestControllers;

namespace TerseControllerTesting.Tests
{
    [TestFixture]
    class ControllerExtensionsShould
    {
        private TestController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new TestController();
        }

        [Test]
        public void Give_controller_modelstate_errors()
        {
            _controller.WithModelErrors();
            Assert.That(_controller.ModelState.IsValid, Is.False);
        } 
    }
}
