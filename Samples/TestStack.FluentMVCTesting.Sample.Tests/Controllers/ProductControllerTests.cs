using System;
using NUnit.Framework;
using TestStack.FluentMVCTesting.Sample.Controllers;

namespace TestStack.FluentMVCTesting.Sample.Tests.Controllers
{
    [TestFixture]
    class ProductControllerTests
    {
        private ProductController _controller;
        
        [SetUp]
        public void SetUp()
        {
           _controller = new ProductController(); 
        }

        [Test]
        public void WhenViewingIndexPage_ThenShouldRedirectToProduct_WithId()
        {
            _controller.WithCallTo(c => c.Index())
                .ShouldRedirectTo(c => c.Product())
                .WithRouteValue("Id"); 
        }

        [Test]
        public void WhenViewingIndexPage_ThenShouldRedirectToProduct_WithIdEqualToOne()
        {
            _controller.WithCallTo(c => c.Index())
                .ShouldRedirectTo(c => c.Product())
                .WithRouteValue("Id", 1);
        }

        [Test]
        public void UnknownUserAction_RedirectsToUser_WithEmptyGuidAndUnknownUserType()
        {
            _controller.WithCallTo(c => c.UnknownUser())
                .ShouldRedirectTo(c => c.User())
                .WithRouteValue("Id", Guid.Empty)
                .WithRouteValue("UserType", UserType.Unknown);
        }
    }
}