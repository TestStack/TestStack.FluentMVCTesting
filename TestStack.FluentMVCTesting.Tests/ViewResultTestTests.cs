//
using System;
using System.Collections.Generic;
using System.Web.Mvc;
//
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
        public void Check_string_message_key_on_viewbag()
        {
            _viewResult.ViewBag.Message = "Hello View Data Message";
            Assert.That(_viewResultTest.WithViewBag<string>("Message"), Is.EqualTo("Hello View Data Message"));
        }

        [Test]
        public void Check_int_message_key_on_viewbag()
        {
            _viewResult.ViewBag.Page = 1;
            Assert.That(_viewResultTest.WithViewBag<int>("Page"), Is.EqualTo(1));
        }

        [Test]
        public void Check_wrong_message_member_name_passed_missing_member_message_for_viewbag()
        {
            _viewResult.ViewBag.Message = "Hello View Data Message";

            var exception = Assert.Throws<MissingMemberException>(() =>
                _viewResultTest.WithViewBag<string>("Message2")
            );

            Assert.That(exception.Message, Is.EqualTo("Member 'ViewBag.Message2' not found."));
        }


        [Test]
        public void Check_string_message_key_on_viewdata()
        {
            _viewResult.ViewData["Message"] = "Hello View Data Message";
            Assert.That(_viewResultTest.WithViewData<string>("Message"), Is.EqualTo("Hello View Data Message"));
        }

        [Test]
        public void Check_int_message_key_on_viewdata()
        {
            _viewResult.ViewData["Page"] = 1;
            Assert.That(_viewResultTest.WithViewData<int>("Page"), Is.EqualTo(1));
        }

        [Test]
        public void Check_wrong_message_key_passed_missing_message_key_for_viewdata()
        {
            _viewResult.ViewData["Message"] = "Hello View Data Message";

            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _viewResultTest.WithViewData<string>("Message2")
            );

            Assert.That(exception.Message, Is.EqualTo("Exception with ViewData, 'Message2' key not found"));
        }

        [Test]
        public void Check_row_count_zero_for_viewdata()
        {
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _viewResultTest.WithViewData<string>("Message")
            );

            Assert.That(exception.Message, Is.EqualTo("Exception with ViewData, 'Message' key not found"));
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
            Assert.That(exception.Message, Is.EqualTo("Expected view model to pass the given condition, but it failed."));
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

    class InvalidViewModel {}
    public class TestViewModel
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
    }
    class ViewTestController : Controller { }
}
