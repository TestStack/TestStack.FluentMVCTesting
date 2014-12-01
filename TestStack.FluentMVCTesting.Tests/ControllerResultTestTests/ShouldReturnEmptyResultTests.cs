using System.Web.Mvc;
using NUnit.Framework;

namespace TestStack.FluentMVCTesting.Tests
{
    partial class ControllerResultTestShould
    {
        [Test]
        public void Check_for_empty_result()
        {
            _controller.WithCallTo(c => c.EmptyResult()).ShouldReturnEmptyResult();
        }

        [Test]
        public void Return_the_empty_result()
        {
            EmptyResult actual = _controller.WithCallTo(c => c.EmptyResult())
                .ShouldReturnEmptyResult();
            Assert.NotNull(actual);
        }
    }
}