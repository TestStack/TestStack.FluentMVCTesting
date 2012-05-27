using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;

namespace TerseControllerTesting.Tests
{
    class ModelErrorTestMetadata : Tuple<string, string, string, ModelErrorTestCall>
    {
        public ModelErrorTestMetadata(string errorMessagePart, string validError1Value, string validError2Value, ModelErrorTestCall testCall)
            : base(errorMessagePart, validError1Value, validError2Value, testCall) {}
    }
    delegate ModelTest<TestViewModel> ModelErrorTestCall(ModelErrorTest<TestViewModel> modelErrorTest, string input);

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

        private ModelErrorTest<TestViewModel> _modelTest;
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
            _modelTest = new ModelErrorTest<TestViewModel>(new ModelTest<TestViewModel>(new ViewTestController()), ErrorKey, _errors);
        }

        [Test]
        [TestCaseSource("_tests")]
        public void Check_for_lack_of_matching_error_message(ModelErrorTestMetadata test)
        {
            var exception = Assert.Throws<ModelErrorAssertionException>(() =>
                test.Item4(_modelTest, NonError)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("{0} {1} '{2}', but instead found '{3}'.", _initialExceptionMessage, test.Item1, NonError, _combinedErrors)));
        }

        [Test]
        [TestCaseSource("_tests")]
        public void Check_for_first_error_message(ModelErrorTestMetadata test)
        {
            test.Item4(_modelTest, test.Item2);
        }

        [Test]
        [TestCaseSource("_tests")]
        public void Check_for_subsequent_error_message(ModelErrorTestMetadata test)
        {
            test.Item4(_modelTest, test.Item3);
        }
    }
}
