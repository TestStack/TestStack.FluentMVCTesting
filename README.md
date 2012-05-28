Fluent MVC Testing
====================================

This library provides a fluent interface for creating terse and expressive tests against ASP.NET MVC controllers.

This library is testing framework agnostic, so you can combine it with the testing library of your choice (e.g. NUnit, xUnit, etc.).

The library is compatible with the AAA testing methodology, although it combines the Act and Assert parts together (but you can have other assertions after the Fluent assertion). See the code examples below for more information.

The motivation behind this library is to provide a way to test MVC actions quickly, tersely and maintainably. Most examples I find on MVC controller testing are incredibly verbose, repetitive and time-consuming to write and maintain. Given how quickly you can write controller actions and how simple they are (assuming you are following best practices and keeping them lean) the time to test them generally isn't worth it given you can glance at most of your controller actions and know they are right or wrong instantly. This library aims to make the time to implement the tests inconsequential and then the value your tests are providing is worth it. The other problem that I've noticed with most examples of controller testing is that there are a lot of magic strings being used to test view and action names; this library also aims to utilise the type system to resolve a lot of those magic strings, thus ensuring your tests are more maintainable and require less re-work when you perform major refactoring of your code.

Installation
------------

You can install this library using NuGet into your Test Library; it will automatically reference System.Web and System.Web.Mvc for you.

If you are using ASP.NET MVC 4 then:

    Install-Package FluentMVCTesting

If you are using ASP.NET MVC 3 then:

    Install-Package FluentMVCTesting.Mvc3

Examples
--------

### Recommended class structure

The following code snippet is an example for how to set up a test class to use FluentMVCTesting using NUnit, this is simply to get you started quickly, in reality you can use it how you like and with any unit testing framework of your choice.

    using MyApp.Controllers;
    using NUnit.Framework;
    using TerseControllerTesting;

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

            [Test]
            public void Render_default_view_for_get_to_index()
            {
                _controller.WithCallTo(c => c.Index()).ShouldRenderDefaultView();
            }
        }
    }

### Basic syntax

As you might have gathered from the previous code snippet the entry point to the library is the `WithCallTo` extension method on the MVC `Controller` class; inside of that method call you simply pass in a lamdba expression that takes the controller to make the action call that you are trying to test. From there you can use intellisense to guide your testing.

### Model binding errors

If you want to test what happens when the default model binding (or indeed any custom model binding or validation that you perform) has left errors in `ModelState` when your action is called you can simply use the `WithModelErrors()` extension method, e.g.:

    _controller.WithModelErrors().WithCallTo(c => c.Index()).ShouldRenderDefaultView();

### Child actions

If you want to check that the action being called has the `[ChildActionOnly]` attribute then use `WithCallToChild()` rather than `WithCallTo()`, e.g.:

    _controller.WithCallToChild(c => c.Index()).ShouldRenderDefaultView();

### Empty result

    _controller.WithCallTo(c => c.Index()).ShouldReturnEmptyResult();

### Redirect to url

    _controller.WithCallTo(c => c.Index()).ShouldRedirectTo("http://www.google.com.au/");

### Redirect to route name

    _controller.WithCallTo(c => c.Index()).ShouldRedirectToRoute("RouteName");

### Redirect to action within the same controller

There are a number of ways that you can specify this test, depending on the signature of the action you are redirecting to.

If you are redirecting to an action that takes no parameters, or takes a single int parameter, then you can use a method group, which is the tersest specification (you don't need to specify the parameters to the action), e.g. if these are two actions in the controller you are testing:

    public ActionResult ActionWithNoParameters()
    {
        return new EmptyResult();
    }
    public ActionResult RedirectToActionWithNoParameters()
    {
        return RedirectToAction("ActionWithNoParameters");
    }

Then you can write this test:

    _controller.WithCallTo(c => c.RedirectToActionWithNoParameters()).ShouldRedirectTo(c => c.ActionWithNoParameters)

I can explicitly define whatever signatures I want to allow this terser syntax, but obviously the different permutations that are possible are mind-boggling and it wouldn't be helpful for any custom types in your project. Unfortunately, despite best efforts I couldn't figure out a way to generically specify these definitions - if anyone has ideas for how to do this let me know!

If you have 1-3 parameters being passed into the action being redirected to then you can still use the method group, but you need to specify the types being passed into the action, e.g. if you have the following controller action being redirected to:

    public ActionResult SomeAction(string param1, int param2, bool param3) {}

Then you can write the following test for the redirect:

    _controller.WithCallTo(c => c.Index()).ShouldRedirectTo<string, int, bool>(c => c.SomeAction)

If you have more than three parameters, or you are uncomfortable with that syntax then you can specify a lambda with a call to the action you want and pass in dummy values for the parameters, e.g. for the previous example:

    _controller.WithCallTo(c => c.Index()).ShouldRedirectTo(c => c.SomeAction(null, 0, false))

You can also pass through a MethodInfo object against the method you are redirecting to, e.g.:

    _controller.WithCallTo(c => c.Index()).ShouldRedirectTo(typeof(HomeController).GetMethod("SomeAction"))

If you use this option (I don't recommend it because it uses a "magic" string so if you change the action name then the string won't change, although at least the test will break because the Method name will no longer be valid; in saying that if you change your parameters more often than the action name this might be a better option) be careful that you don't get an AmbiguousMatchException if there are multiple actions with that name.

At this stage there isn't support for the `[ActionName()]` attribute or simply passing through a string to check against the action name, but if either are important to you feel free to add an issue for this GitHub project and I can add them.
