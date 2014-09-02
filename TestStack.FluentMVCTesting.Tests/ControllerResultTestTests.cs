using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web.Mvc;
using NUnit.Framework;
using TestStack.FluentMVCTesting.Tests.TestControllers;
using System.Text;


namespace TestStack.FluentMVCTesting.Tests
{
    [TestFixture]
    class ControllerResultTestShould
    {
        #region Test cases
        #pragma warning disable 169
        // Expected action return types for the different types of assertions
        private static readonly List<Tuple<string, TestAction>> ReturnTypes = new List<Tuple<string, TestAction>>
        {
            ReturnType<EmptyResult>(t => t.ShouldReturnEmptyResult()),
            ReturnType<RedirectResult>(t => t.ShouldRedirectTo("")),
            ReturnType<RedirectToRouteResult>(t => t.ShouldRedirectTo(c => c.EmptyResult)),
            ReturnType<RedirectToRouteResult>(t => t.ShouldRedirectTo<SomeOtherController>(c => c.SomeAction())),
            ReturnType<ViewResult>(t => t.ShouldRenderView("")),
            ReturnType<ViewResult>(t => t.ShouldRenderDefaultView()),
            ReturnType<PartialViewResult>(t => t.ShouldRenderPartialView("")),
            ReturnType<PartialViewResult>(t => t.ShouldRenderDefaultPartialView()),
            ReturnType<FileContentResult>(t => t.ShouldRenderFileContents()),
            ReturnType<FileContentResult>(t => t.ShouldRenderFileContents(new byte[0])),
            ReturnType<FileContentResult>(t => t.ShouldRenderFileContents(new byte[0], "")),
            ReturnType<FileContentResult>(t => t.ShouldRenderFileContents("")),
            ReturnType<FileContentResult>(t => t.ShouldRenderFileContents("", "")),
            ReturnType<FileContentResult>(t => t.ShouldRenderFileContents("", "", Encoding.UTF8)),
            ReturnType<FilePathResult>(t => t.ShouldRenderFilePath()),
            ReturnType<FilePathResult>(t => t.ShouldRenderFilePath("")),
            ReturnType<FilePathResult>(t => t.ShouldRenderFilePath("", "")),
            ReturnType<FileStreamResult>(t => t.ShouldRenderFileStream()),
            ReturnType<FileStreamResult>(t => t.ShouldRenderFileStream(new MemoryStream())),
            ReturnType<FileStreamResult>(t => t.ShouldRenderFileStream(contentType: "")),
            ReturnType<FileStreamResult>(t => t.ShouldRenderFileStream(new MemoryStream(), "")),
            ReturnType<FileStreamResult>(t => t.ShouldRenderFileStream(new byte[0])),
            ReturnType<FileStreamResult>(t => t.ShouldRenderFileStream(new byte[0], "")),
            ReturnType<FileStreamResult>(t => t.ShouldRenderFileStream("")),
            ReturnType<FileStreamResult>(t => t.ShouldRenderFileStream("", "")),
            ReturnType<FileStreamResult>(t => t.ShouldRenderFileStream("", "", Encoding.UTF8)),
            ReturnType<FileResult>(t => t.ShouldRenderAnyFile()),
            ReturnType<HttpStatusCodeResult>(t => t.ShouldGiveHttpStatus()),
            ReturnType<JsonResult>(t => t.ShouldReturnJson()),
        };
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
        #pragma warning restore 169
        #endregion

        #region Setup
        public class RedirectToActionTestMetadata : Tuple<string, TestAction, Expression<Func<ControllerResultTestController, ActionResult>>>
        {
            public RedirectToActionTestMetadata(string expectedMethodName, TestAction testCall, Expression<Func<ControllerResultTestController, ActionResult>> validControllerActionCall)
                : base(expectedMethodName, testCall, validControllerActionCall) { }
        }
        private ControllerResultTestController _controller;
        public delegate void TestAction(ControllerResultTest<ControllerResultTestController> testClass);
        private static Tuple<string, TestAction> ReturnType<T>(TestAction a)
        {
            return new Tuple<string, TestAction>(typeof(T).Name, a);
        }
        private static RedirectToActionTestMetadata ActionRedirect(string s, TestAction a, Expression<Func<ControllerResultTestController, ActionResult>> c)
        {
            return new RedirectToActionTestMetadata(s, a, c);
        }

        [SetUp]
        public void Setup()
        {
            _controller = new ControllerResultTestController();
        }
        #endregion

        #region General tests
        [Test]
        [TestCaseSource("ReturnTypes")]
        public void Check_return_type(Tuple<string, TestAction> test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.RandomResult()))
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected action result to be a {0}, but instead received a RandomResult.", test.Item1)));
        }

        [Test]
        [TestCaseSource("ReturnTypes")]
        public void Check_null_return(Tuple<string, TestAction> test)
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                test.Item2(_controller.WithCallTo(c => c.Null()))
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Received null action result when expecting {0}.", test.Item1)));
        }

        [Test]
        public void Check_for_empty_result()
        {
            _controller.WithCallTo(c => c.EmptyResult()).ShouldReturnEmptyResult();
        }
        #endregion

        #region Redirect tests
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
        public void Check_for_redirect_to_action_with_non_specified_controller()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.RedirectToAnotherActionNoController()).ShouldRedirectTo<SomeOtherController>(c => c.SomeOtherAction())
            );
            Assert.That(exception.Message, Is.EqualTo("Expected redirect to action 'SomeOtherAction' in 'SomeOther' controller, but instead was given redirect to action 'SomeAction' within the same controller."));
        }
        #endregion

        #region View tests
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
        #endregion

        #region File tests

        [Test]
        public void Check_for_any_file_result()
        {
            _controller.WithCallTo(c => c.EmptyFile()).ShouldRenderAnyFile();
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderAnyFile();
            _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderAnyFile();
        }

        [Test]
        public void Check_for_any_file_result_and_check_content_type()
        {
            _controller.WithCallTo(c => c.EmptyFile()).ShouldRenderAnyFile(ControllerResultTestController.FileContentType);
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderAnyFile(ControllerResultTestController.FileContentType);
            _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderAnyFile(ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_any_file_result_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyFile()).ShouldRenderAnyFile(contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_content_result()
        {
            _controller.WithCallTo(c => c.EmptyFile()).ShouldRenderFileContents();
        }

        [Test]
        public void Check_for_file_content_result_and_check_binary_content()
        {
            _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(ControllerResultTestController.BinaryFileContents);
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_binary_content()
        {
            byte[] contents = { 1, 2 };
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(contents));

            Assert.True(exception.Message.StartsWith("Expected file contents to be equal to ["));
            Assert.True(exception.Message.EndsWith("]."));
            Assert.True(string.Join(", ", contents).All(exception.Message.Contains));
            Assert.True(string.Join(", ", ControllerResultTestController.BinaryFileContents).All(exception.Message.Contains));
        }

        [Test]
        public void Check_for_file_content_result_and_check_binary_content_and_check_content_type()
        {
            _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(ControllerResultTestController.BinaryFileContents, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(ControllerResultTestController.BinaryFileContents, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_binary_content_and_check_invalid_content_type()
        {
            byte[] contents = { 1, 2 };
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryFile()).ShouldRenderFileContents(contents, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_contents()
        {
            _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent);
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_textual_contents()
        {
            const string contents = "dummy contents";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(contents));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file contents to be \"{0}\", but instead was \"{1}\".", contents, ControllerResultTestController.TextualFileContent)));
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_and_check_content_result()
        {
            _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_and_check_invalid_content_typet()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_content_result_and_check_invalid_textual_content_and_check_invalid_content_type()
        {
            const string contents = "dummy content";
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(contents, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_using_given_char_encoding()
        {
            var encoding = Encoding.BigEndianUnicode;

            _controller.WithCallTo(c => c.TextualFile(encoding))
                .ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, encoding: encoding);
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_using_given_char_encoding_and_check_content_type()
        {
            var encoding = Encoding.BigEndianUnicode;

            _controller.WithCallTo(c => c.TextualFile(encoding)).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType, encoding);
        }

        [Test]
        public void Check_for_file_content_result_and_check_textual_content_using_invalid_given_char_encoding()
        {
            Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualFile()).ShouldRenderFileContents(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType, Encoding.BigEndianUnicode));
        }

        [Test]
        public void Check_for_file_stream_result()
        {
            _controller.WithCallTo(c => c.EmptyStream())
                .ShouldRenderFileStream();
        }

        [Test]
        public void Check_for_file_stream_result_and_check_stream_content()
        {
            _controller.WithCallTo(c => c.EmptyStream())
                .ShouldRenderFileStream(ControllerResultTestController.EmptyStreamContents);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_stream_content()
        {
            var buffer = new byte[] { 1, 2 };
            var stream = new MemoryStream(buffer);

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderFileStream(stream));

            var expected = string.Format("[{0}]", string.Join(", ", buffer));
            var actual   = string.Format("[{0}]", string.Join(", ", ControllerResultTestController.EmptyFileBuffer));
            var message  = string.Format("Expected stream contents to be equal to {0}, but instead was given {1}.", expected, actual);

            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void Check_for_file_stream_result_with_populated_file_and_check_invalid_stream_content()
        {
            var buffer = new byte[] { 1, 2 };
            var stream = new MemoryStream(buffer);

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryStream()).ShouldRenderFileStream(stream));

            var expected = string.Format("[{0}]", string.Join(", ", buffer));
            var actual   = string.Format("[{0}]", string.Join(", ", ControllerResultTestController.BinaryFileContents));
            var message  = string.Format("Expected stream contents to be equal to {0}, but instead was given {1}.", expected, actual);

            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_content_type()
        {
            _controller.WithCallTo(c => c.EmptyStream())
                .ShouldRenderFileStream(ControllerResultTestController.EmptyStreamContents,ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderFileStream(ControllerResultTestController.EmptyStreamContents, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format(
                "Expected stream to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_stream_content_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";
            var buffer = new byte[] { 1, 2 };
            var stream = new MemoryStream(buffer);

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyStream()).ShouldRenderFileStream(stream, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_binary_content()
        {
            _controller.WithCallTo(c => c.BinaryStream())
                .ShouldRenderFileStream(ControllerResultTestController.BinaryFileContents);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_binary_content()
        {
            var content = new byte[] { 1, 2 };

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryStream()).ShouldRenderFileStream(content));

            var expected = string.Format("[{0}]", string.Join(", ", content));
            var actual   = string.Format("[{0}]", string.Join(", ", ControllerResultTestController.BinaryFileContents));
            var message  = string.Format("Expected stream contents to be equal to {0}, but instead was given {1}.", expected, actual);

            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_binary_content_and_check_content_type()
        {
            _controller.WithCallTo(c => c.BinaryStream())
                .ShouldRenderFileStream(ControllerResultTestController.BinaryStreamContents, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_binary_content_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryStream()).ShouldRenderFileStream(ControllerResultTestController.BinaryFileContents, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected stream to be of content type '{0}', but instead was given '{1}'.",
                    contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_binary_content_and_check_invalid_content_type()
        {
            var content = new byte[] { 1, 2 };
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.BinaryStream()).ShouldRenderFileStream(content, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content()
        {
            _controller.WithCallTo(c => c.TextualStream())
                .ShouldRenderFileStream(ControllerResultTestController.TextualFileContent);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_textual_content()
        {
            const string contents = "dummy contents";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualStream()).ShouldRenderFileStream(contents));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file contents to be \"{0}\", but instead was \"{1}\".", contents, ControllerResultTestController.TextualFileContent)));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_and_check_content_type()
        {
            _controller.WithCallTo(c => c.TextualStream())
                .ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualStream()).ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected stream to be of content type '{0}', but instead was given '{1}'.",
                    contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_invalid_textual_content_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";
            const string content = "dummy contents";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualStream()).ShouldRenderFileStream(content, contentType));

            // Assert that the content type validation occurs before that of the actual contents.
            Assert.That(exception.Message.Contains("content type"));
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_using_given_char_encoding()
        {
            var encoding = Encoding.BigEndianUnicode;

            _controller.WithCallTo(c => c.TextualStream(encoding))
                .ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, encoding: encoding);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_using_given_char_encoding_and_check_content_type()
        {
            var encoding = Encoding.BigEndianUnicode;

            _controller.WithCallTo(c => c.TextualStream(encoding))
                .ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, ControllerResultTestController.FileContentType, encoding);
        }

        [Test]
        public void Check_for_file_stream_result_and_check_textual_content_using_invalid_given_char_encoding_and_check_content_type()
        {
            Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.TextualStream()).ShouldRenderFileStream(ControllerResultTestController.TextualFileContent, encoding: Encoding.BigEndianUnicode));
        }

        [Test]
        public void Check_for_file_path_result()
        {
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath();
        }

        [Test]
        public void Check_for_file_path_result_and_check_file_name()
        {
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(ControllerResultTestController.FileName);
        }

        [Test]
        public void Check_for_file_path_result_and_check_invalid_file_name()
        {
            const string name = "dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(name));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file name to be '{0}', but instead was given '{1}'.", name, ControllerResultTestController.FileName)));
        }

        [Test]
        public void Check_for_file_path_result_and_check_file_name_and_check_content_type()
        {
            _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(ControllerResultTestController.FileName, ControllerResultTestController.FileContentType);
        }

        [Test]
        public void Check_for_file_path_result_and_check_file_name_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(ControllerResultTestController.FileName, contentType));

            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected file to be of content type '{0}', but instead was given '{1}'.", contentType, ControllerResultTestController.FileContentType)));
        }

        [Test]
        public void Check_for_file_path_result_and_check_invalid_file_name_and_check_invalid_content_type()
        {
            const string contentType = "application/dummy";
            const string name = "dummy";

            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.EmptyFilePath()).ShouldRenderFilePath(name, contentType));

            // Assert that the content type validation occurs before that of the file name.
            Assert.That(exception.Message.Contains("content type"));
        }

        #endregion

        #region HTTP Status tests
        [Test]
        public void Check_for_http_not_found()
        {
            _controller.WithCallTo(c => c.NotFound()).ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [Test]
        public void Check_for_http_status()
        {
            _controller.WithCallTo(c => c.StatusCode()).ShouldGiveHttpStatus(ControllerResultTestController.Code);
        }

        [Test]
        public void Check_for_invalid_http_status()
        {
            var exception = Assert.Throws<ActionResultAssertionException>(() =>
                _controller.WithCallTo(c => c.StatusCode()).ShouldGiveHttpStatus(ControllerResultTestController.Code+1)
            );
            Assert.That(exception.Message, Is.EqualTo(string.Format("Expected HTTP status code to be '{0}', but instead received a '{1}'.", ControllerResultTestController.Code + 1, ControllerResultTestController.Code)));
        }
        #endregion

        #region Json tests
        [Test]
        public void Allow_the_object_that_is_returned_to_be_checked()
        {
            _controller.WithCallTo(c => c.Json()).ShouldReturnJson(d => Assert.That(d, Is.EqualTo(ControllerResultTestController.JsonValue)));
        }
        #endregion
    }
}
