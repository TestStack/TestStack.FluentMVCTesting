using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using TestStack.FluentMVCTesting.Tests.TestControllers;

namespace TestStack.FluentMVCTesting.Tests
{
    [TestFixture]
    partial class ControllerResultTestShould
    {
        private ControllerResultTestController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new ControllerResultTestController();
        }

        #region General Tests
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
            ReturnType<ContentResult>(t => t.ShouldReturnContent()),
            ReturnType<ContentResult>(t => t.ShouldReturnContent("")),
            ReturnType<ContentResult>(t => t.ShouldReturnContent("", "")),
            ReturnType<ContentResult>(t => t.ShouldReturnContent("", "", Encoding.UTF8))
        };

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
        #endregion
    }
}
