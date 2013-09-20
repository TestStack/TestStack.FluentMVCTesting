using System.Web.Mvc;
using System.Web.Routing;
using NSubstitute;
using NUnit.Framework;
using TestStack.FluentMVCTesting.Sample.Controllers;
using TestStack.FluentMVCTesting.Sample.Models;
using TestStack.FluentMVCTesting.Sample.Services;

namespace TestStack.FluentMVCTesting.Sample.Tests.Controllers
{
    class AccountControllerTests
    {
        #region Setup
        private AccountController _controller;
        private IAuthenticationService _authenticationService;

        [SetUp]
        public void Setup()
        {
            _authenticationService = Substitute.For<IAuthenticationService>();
            _controller = new AccountController(_authenticationService)
            {
                Url = new UrlHelper(Substitute.For<RequestContext>())
            };
        }
        #endregion

        #region Login

        [Test]
        public void WhenViewingLoginPage_ThenShowDefaultViewWithReturnUrl()
        {
            const string returnUrl = "http://www.google.com.au/";

            _controller.WithCallTo(c => c.Login(returnUrl))
                .ShouldRenderDefaultView();

            Assert.That(_controller.ViewBag.ReturnUrl, Is.EqualTo(returnUrl));
        }

        [Test]
        public void GivenInvalidSubmission_WhenPostingLoginDetails_ThenShowDefaultViewWithInvalidModelAndReturnUrl()
        {
            var vm = new LoginModel();
            const string returnUrl = "http://www.google.com.au/";

            _controller.WithModelErrors()
                .WithCallTo(c => c.Login(vm, returnUrl))
                .ShouldRenderDefaultView()
                .WithModel(vm);

            Assert.That(_controller.ViewBag.ReturnUrl, Is.EqualTo(returnUrl));
        }

        [Test]
        public void GivenValidSubmissionButIncorrectDetails_WhenPostingLoginDetails_ThenShowDefaultViewWithInvalidModelAndReturnUrlAndErrorMessage()
        {
            var vm = new LoginModel();
            const string returnUrl = "http://www.google.com.au/";
            _authenticationService.Login(vm).Returns(false);

            _controller.WithCallTo(c => c.Login(vm, returnUrl))
                .ShouldRenderDefaultView()
                .WithModel(vm)
                .AndModelError("").ThatEquals("The user name or password provided is incorrect.");

            Assert.That(_controller.ViewBag.ReturnUrl, Is.EqualTo(returnUrl));
        }

        [Test]
        public void GivenLocalReturnUrlAndValidSubmission_WhenPostingLoginDetails_ThenLogUserInAndRedirectToReturnUrl()
        {
            var vm = new LoginModel();
            const string returnUrl = "/localurl";
            _authenticationService.Login(vm).Returns(true);

            _controller.WithCallTo(c => c.Login(vm, returnUrl))
                .ShouldRedirectTo(returnUrl);
        }

        [Test]
        public void GivenNonLocalReturnUrlAndValidSubmission_WhenPostingLoginDetails_ThenLogUserInAndRedirectToHomepage()
        {
            var vm = new LoginModel();
            const string returnUrl = "http://www.google.com.au/";
            _authenticationService.Login(vm).Returns(true);

            _controller.WithCallTo(c => c.Login(vm, returnUrl))
                .ShouldRedirectTo<HomeController>(c => c.Index());
        }

        [Test]
        public void GivenNoReturnUrlAndValidSubmission_WhenPostingLoginDetails_ThenLogUserInAndRedirectToHomepage([Values(null, "")] string returnUrl)
        {
            var vm = new LoginModel();
            _authenticationService.Login(vm).Returns(true);

            _controller.WithCallTo(c => c.Login(vm, returnUrl))
                .ShouldRedirectTo<HomeController>(c => c.Index());
        }

        #endregion
    }
}
