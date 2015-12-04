TestStack.FluentMVCTesting
====================================

This library provides a fluent interface for creating terse and expressive tests against ASP.NET MVC controllers. This library is part of [TestStack](http://teststack.net/).

This library is testing framework agnostic, so you can combine it with the testing library of your choice (e.g. NUnit, xUnit, etc.).

The library is compatible with the AAA testing methodology, although it combines the Act and Assert parts together (but you can also have other assertions after the Fluent assertion). See the code examples below for more information.

Documentation
-------------

Please see [the documentation](http://fluentmvctesting.teststack.net/) for full installation and usage instructions.


Installation
------------

You can install this library using NuGet into your Test Library; it will automatically reference System.Web and System.Web.Mvc (via NuGet packages, sorry it also installs a heap of other dependencies - it would be cool if Microsoft provided a package with just the MVC dll!) for you.

If you are using ASP.NET MVC 5 (.NET 4.5+) then:

    Install-Package TestStack.FluentMVCTesting

If you are using ASP.NET MVC 4 (.NET 4.0+) then:

	Install-Package TestStack.FluentMVCTesting.Mvc4

If you are using ASP.NET MVC 3 (.NET 4.0+) then:

    Install-Package TestStack.FluentMVCTesting.Mvc3

Known Issues
------------

If you get the following exception:

    System.Security.VerificationException : Method FluentMVCTesting.ControllerExtensions.WithCallTo: type argument 'MyApp.Controllers.MyController' violates the constraint of type parameter 'T'.

It means you are referencing a version of System.Web.Mvc that isn't compatible with the one that was used to build the dll that was generated for the NuGet package. Ensure that you are using the correct package for your version of MVC and that you are using the [AspNetMvc packages on nuget.org](https://nuget.org/packages/aspnetmvc) rather than the dll from the GAC.

Show me the code!
-----------------

Make sure to check out [the documentation](http://fluentmvctesting.teststack.net/) for full usage instructions.

Say you set up the following test class (this example with NUnit, but it will work for any test framework).

```c#
using MyApp.Controllers;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace MyApp.Tests.Controllers
{
    [TestFixture]
    class HomeControllerShould
    {
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new HomeController();
        }
    }
}
```

Then you can create a test like this:

```c#
[Test]
public void Render_default_view_for_get_to_index()
{
    _controller.WithCallTo(c => c.Index()).ShouldRenderDefaultView();
}
```

This checks that when `_controller.Index()` is called then the `ActionResult` that is returned is of type `ViewResult` and that the view name returned is either "Index" or "" (the default view for the Index action); easy huh?

Here are some other random examples of assertions that you can make (see the documentation for the full list):

```c#
var vm = new SomeViewModel();
_controller.WithModelErrors().WithCallTo(c => c.Index(vm))
    .ShouldRenderDefaultView()
    .WithModel(vm);

_controller.WithCallTo(c => c.Index())
    .ShouldRenderDefaultView()
    .WithModel<ModelType>()
    .AndModelErrorFor(m => m.Property1).ThatEquals("The error message.")
    .AndModelErrorFor(m => m.Property2);

_controller.WithCallTo(c => c.Index())
    .ShouldRenderView("ViewName");
    
_controller.WithCallTo(c => c.Index())
    .ShouldReturnEmptyResult();
    
_controller.WithCallTo(c => c.Index())
    .ShouldRedirectTo("http://www.google.com.au/");
    
_controller.WithCallTo(c => c.ActionWithRedirectToAction())
    .ShouldRedirectTo(c => c.ActionInSameController);
    
_controller.WithCallTo(c => c.Index())
    .ShouldRedirectTo<SomeOtherController>(c => c.SomeAction());
    
_controller.WithCallTo(c => c.Index())
    .ShouldRenderAnyFile("content/type");

_controller.WithCallTo(c => c.Index())
    .ShouldRenderFileContents("text");

_controller.WithCallTo(c => c.Index())
    .ShouldReturnContent("expected content");
    
_controller.WithCallTo(c => c.Index())
    .ShouldGiveHttpStatus(404);
    
_controller.WithCallTo(c => c.Index()).ShouldReturnJson(data =>
{
    Assert.That(data.SomeProperty, Is.EqualTo("SomeValue");
});
```

Any questions, comments or additions?
-------------------------------------

Leave an issue on the [issues page](https://github.com/TestStack/TestStack.FluentMVCTesting/issues) or send a [pull request](https://github.com/TestStack/TestStack.FluentMVCTesting/pulls).
