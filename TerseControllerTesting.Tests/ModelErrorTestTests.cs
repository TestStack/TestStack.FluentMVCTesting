using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace TerseControllerTesting.Tests
{
    class ModelErrorTestMetadata : Tuple<string, string, string, ModelErrorTestCall>
    {
        public ModelErrorTestMetadata(string errorMessagePart, string validError1Value, string validError2Value, ModelErrorTestCall testCall)
            : base(errorMessagePart, validError1Value, validError2Value, testCall) {}
    }
    delegate IModelTest<TestViewModel> ModelErrorTestCall(ModelErrorTest<TestViewModel> modelErrorTest, string input);

    [TestFixture]
    class ModelErrorTestShould
    {
        // List of test metadata:
        //  Error message part, valid value for first error, valid value for second error, lambda with method to call
        #pragma warning disable 169
        private static List<ModelErrorTestMetadata> _tests = new List<ModelErrorTestMetadata>
        {
            Test("to be", Error1, Error2, (t, s) => t.ThatEquals(s)),
            Test("to start with", Error1.Substring(0, 3), Error2.Substring(0, 3), (t, s) => t.BeginningWith(s)),
            Test("to end with", Error1.Substring(5), Error2.Substring(5), (t, s) => t.EndingWith(s)),
            Test("to contain", Error1.Substring(2, 7), Error2.Substring(2, 7), (t, s) => t.Containing(s)),
        };
        #pragma warning restore 169

        private static ModelErrorTestMetadata Test(string message, string error1, string error2, ModelErrorTestCall testCall)
        {
            return new ModelErrorTestMetadata(message, error1, error2, testCall);
        }

        private ModelErrorTest<TestViewModel> _modelErrorTest;
        private IModelTest<TestViewModel> _modelTest;

        private const string ErrorKey = "Key";
        private const string Error1 = "Error message 1";
        private const string Error2 = "Error message 2";
        private const string NonError = "Random error";
        private readonly string _combinedErrors = string.Format("{0}, {1}", Error1, Error2);
        private readonly string _initialExceptionMessage = string.Format("Expected error message for key '{0}'", ErrorKey);
        private readonly IEnumerable<ModelError> _errors = new[]
        {
            new ModelError(Error1),
            new ModelError(Error2)
        };

        [SetUp]
        public void Setup()
        {
            _modelTest = Substitute.For<IModelTest<TestViewModel>>();
            _modelErrorTest = new ModelErrorTest<TestViewModel>(_modelTest, ErrorKey, _errors);
        }

        [Test]
        [TestCaseSource("_tests")]
        public void Check_for_lack_of_matching_error_message(ModelErrorTestMetadata test)
        {
            var exception = Assert.Throws<ModelErrorAssertionException>(() =>
                test.Item4(_modelErrorTest, NonError)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("{0} {1} '{2}', but instead found '{3}'.", _initialExceptionMessage, test.Item1, NonError, _combinedErrors)));
        }

        [Test]
        [TestCaseSource("_tests")]
        public void Check_for_first_error_message(ModelErrorTestMetadata test)
        {
            test.Item4(_modelErrorTest, test.Item2);
        }

        [Test]
        [TestCaseSource("_tests")]
        public void Check_for_subsequent_error_message(ModelErrorTestMetadata test)
        {
            test.Item4(_modelErrorTest, test.Item3);
        }

        [Test]
        [TestCaseSource("_tests")]
        public void Allow_for_chained_test_calls(ModelErrorTestMetadata test)
        {
            Assert.That(test.Item4(_modelErrorTest, test.Item2), Is.EqualTo(_modelTest));
        }

        [Test]
        public void Chain_call_to_check_model_error_in_property()
        {
            var returnVal = Substitute.For<IModelErrorTest<TestViewModel>>();
            _modelTest.AndModelErrorFor(Arg.Any<Expression<Func<TestViewModel, string>>>()).Returns(returnVal);

            Assert.That(_modelErrorTest.AndModelErrorFor(m => m.Property1), Is.EqualTo(returnVal));
        }

        [Test]
        public void Chain_call_to_check_model_error_by_key()
        {
            var returnVal = Substitute.For<IModelErrorTest<TestViewModel>>();
            _modelTest.AndModelError("Key").Returns(returnVal);

            Assert.That(_modelErrorTest.AndModelError("Key"), Is.EqualTo(returnVal));
        }
    }
}
