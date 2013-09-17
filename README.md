TestStack.FluentMVCTesting
====================================

This library provides a fluent interface for creating terse and expressive tests against ASP.NET MVC controllers. This library is part of [TestStack](http://teststack.net/).

This library is testing framework agnostic, so you can combine it with the testing library of your choice (e.g. NUnit, xUnit, etc.).

The library is compatible with the AAA testing methodology, although it combines the Act and Assert parts together (but you can also have other assertions after the Fluent assertion). See the code examples below for more information.

The motivation behind this library is to provide a way to test MVC actions quickly, tersely and maintainably. Most examples we find on MVC controller testing are incredibly verbose, repetitive and time-consuming to write and maintain. Given how quickly you can write controller actions and how simple they are (assuming you are following best practices and keeping them lean) the time to test them generally isn't worth it given you can glance at most of your controller actions and know they are right or wrong instantly. This library aims to make the time to implement the tests inconsequential and then the value your tests are providing is worth it. The other problem that we've noticed with most examples of controller testing is that there are a lot of magic strings being used to test view and action names; this library also aims to (where possible) utilise the type system to resolve a lot of those magic strings, thus ensuring your tests are more maintainable and require less re-work when you perform major refactoring of your code.

This library was inspired by the [MVCContrib.TestHelper](http://mvccontrib.codeplex.com/wikipage?title=TestHelper) library.

Documentation
-------------

Please see [the documentation](http://docs.teststack.net/fluentmvctesting/) for full installation and usage instructions.


Installation
------------

You can install this library using NuGet into your Test Library; it will automatically reference System.Web and System.Web.Mvc (via NuGet packages, sorry it also installs a heap of other dependencies - it would be cool if Microsoft provided a package with just the MVC dll!) for you.

If you are using ASP.NET MVC 4 then:

    Install-Package TestStack.FluentMVCTesting

If you are using ASP.NET MVC 3 then:

    Install-Package TestStack.FluentMVCTesting.Mvc3

Known Issues
------------

If you get the following exception:

    System.Security.VerificationException : Method FluentMVCTesting.ControllerExtensions.WithCallTo: type argument 'MyApp.Controllers.MyController' violates the constraint of type parameter 'T'.

It means you are referencing a version of System.Web.Mvc that isn't compatible with the one that was used to build the dll that was generated for the NuGet package. Ensure that you are using the correct package for your version of MVC and that you are using the [AspNetMvc packages on nuget.org](https://nuget.org/packages/aspnetmvc) rather than the dll from the GAC.

Show me the code!
-----------------

Make sure to check out [the documentation](http://docs.teststack.net/fluentmvctesting/) for full usage instructions.

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
    .ShouldRenderFile("text/plain");
    
_controller.WithCallTo(c => c.Index())
    .ShouldGiveHttpStatus(404);
    
_controller.WithCallTo(c => c.Index()).ShouldReturnJson(data =>
    {
        Assert.That(data.SomeProperty, Is.EqualTo("SomeValue");
    }
);
```

Any questions, comments or additions?
-------------------------------------

Leave an issue on the [issues page](https://github.com/TestStack/TestStack.FluentMVCTesting/issues) or send a [pull request](https://github.com/TestStack/TestStack.FluentMVCTesting/pulls).
