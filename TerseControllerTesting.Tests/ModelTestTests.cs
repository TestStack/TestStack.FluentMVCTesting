using NUnit.Framework;

namespace TerseControllerTesting.Tests
{
    [TestFixture]
    class ModelTestShould
    {
        private ModelTest<TestViewModel> _modelTest;
        private ViewTestController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new ViewTestController();
            _modelTest = new ModelTest<TestViewModel>(_controller);
        }

        [Test]
        public void Check_for_lack_of_model_errors()
        {
            _modelTest.AndNoModelErrors();
        }

        [Test]
        public void Check_for_unexpected_model_errors()
        {
            _controller.ModelState.AddModelError("key", "error");
            var exception = Assert.Throws<ViewResultModelAssertionException>(() =>
                _modelTest.AndNoModelErrors()
            );
            Assert.That(exception.Message, Is.EqualTo("Expected controller 'ViewTestController' to have no model errors, but it had some."));
        }

        [Test]
        public void Check_for_model_error_in_key()
        {
            _controller.ModelState.AddModelError("Key", "error");
            _modelTest.AndModelError("Key");
        }

        [Test]
        public void Check_for_unexpected_lack_of_model_error_in_key()
        {
            var exception = Assert.Throws<ViewResultModelAssertionException>(() =>
                _modelTest.AndModelError("Key")
            );
            Assert.That(exception.Message, Is.EqualTo("Expected controller 'ViewTestController' to have a model error against key 'Key', but none found."));
        }

        [Test]
        public void Check_for_model_error_in_property()
        {
            _controller.ModelState.AddModelError("Property1", "error");
            _modelTest.AndModelErrorFor(m => m.Property1);
        }

        [Test]
        public void Check_for_execpected_lack_of_model_error_in_property()
        {
            var exception = Assert.Throws<ViewResultModelAssertionException>(() =>
                _modelTest.AndModelErrorFor(m => m.Property1)
            );
            Assert.That(exception.Message, Is.EqualTo("Expected controller 'ViewTestController' to have a model error for member 'Property1', but none found."));
        }
    }
}
