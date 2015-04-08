using System.Text.RegularExpressions;
using System.Web.Mvc;
using NUnit.Framework;

namespace TestStack.FluentMVCTesting.Tests
{
    [TestFixture]
    class ViewResultTestShould
    {
        private ViewResultTest _viewResultTest;
        private TestViewModel _model;
        private ViewResult _viewResult;

        [SetUp]
        public void Setup()
        {
            _viewResult = new ViewResult();
            _model = new TestViewModel { Property1 = "test", Property2 = 3 };
            _viewResult.ViewData.Model = _model;
            _viewResultTest = new ViewResultTest(_viewResult, new ViewTestController());
        }

        [Test]
        public void Check_the_type_of_model()
        {
            _viewResultTest.WithModel<TestViewModel>();
        }

        [Test]
        public void Check_for_null_model()
        {
            _viewResult.ViewData.Model = null;
            var exception = Assert.Throws<ViewResultModelAssertionException>(() =>
                _viewResultTest.WithModel<TestViewModel>()
            );
            Assert.That(exception.Message, Is.EqualTo("Expected view model, but was null."));
        }

        [Test]
        public void Check_for_invalid_model_type()
        {
            var exception = Assert.Throws<ViewResultModelAssertionException>(() =>
                _viewResultTest.WithModel<InvalidViewModel>()
            );
            Assert.That(exception.Message, Is.EqualTo("Expected view model to be of type 'InvalidViewModel', but it is actually of type 'TestViewModel'."));
        }

        [Test]
        public void Check_for_model_by_reference()
        {
            _viewResultTest.WithModel(_model);
        }

        [Test]
        public void Check_for_invalid_model_by_reference()
        {
            var exception = Assert.Throws<ViewResultModelAssertionException>(() =>
                _viewResultTest.WithModel(new TestViewModel())
            );
            Assert.That(exception.Message, Is.EqualTo("Expected view model to be the given model, but in fact it was a different model."));
        }

        [Test]
        public void Check_for_model_using_predicate()
        {
            _viewResultTest.WithModel<TestViewModel>(m => m.Property1 == _model.Property1 && m.Property2 == _model.Property2);
        }

        [Test]
        public void Check_for_invalid_model_using_predicate()
        {
            var exception = Assert.Throws<ViewResultModelAssertionException>(() =>
                _viewResultTest.WithModel<TestViewModel>(m => m.Property1 == null)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected view model {{\"Property1\":\"{0}\",\"Property2\":{1}}} to pass the given condition ((m) => (m.Property1 == null)), but it failed.", _model.Property1, _model.Property2)));
        }

        [Test]
        public void Check_for_invalid_model_using_predicate_with_conditional_or()
        {
            var exception = Assert.Throws<ViewResultModelAssertionException>(() =>
                _viewResultTest.WithModel<TestViewModel>(m => m.Property1 == null || m.Property2 == 1)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected view model {{\"Property1\":\"{0}\",\"Property2\":{1}}} to pass the given condition ((m) => ((m.Property1 == null) || (m.Property2 == 1))), but it failed.", _model.Property1, _model.Property2)));
        }

        [Test]
        public void Check_for_invalid_model_using_predicate_with_primitive_operand()
        {
            _viewResult.ViewData.Model = "abc";
            var exception = Assert.Throws<ViewResultModelAssertionException>(() =>
                _viewResultTest.WithModel<string>(m => m == "ab")
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected view model \"{0}\" to pass the given condition ((m) => (m == \"ab\")), but it failed.", _viewResult.ViewData.Model)));
        }

        [Test]
        public void Allow_for_assertions_on_the_model()
        {
            _viewResultTest.WithModel<TestViewModel>(m =>
                {
                    Assert.That(m.Property1, Is.EqualTo(_model.Property1));
                    Assert.That(m.Property2, Is.EqualTo(_model.Property2));
                }
            );
        }

        [Test]
        public void Run_any_assertions_on_the_model()
        {
            Assert.Throws<AssertionException>(() =>
                _viewResultTest.WithModel<TestViewModel>(m => Assert.That(m.Property1, Is.Null))
            );
        }
    }

    class InvalidViewModel { }
    public class TestViewModel
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
    }
    class ViewTestController : Controller { }
}
