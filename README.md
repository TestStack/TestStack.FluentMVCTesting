TestStack.FluentMVCTesting
====================================

This library provides a fluent interface for creating terse and expressive tests against ASP.NET MVC controllers. This library is part of [TestStack](http://teststack.github.com/).

This library is testing framework agnostic, so you can combine it with the testing library of your choice (e.g. NUnit, xUnit, etc.).

The library is compatible with the AAA testing methodology, although it combines the Act and Assert parts together (but you can also have other assertions after the Fluent assertion). See the code examples below for more information.

The motivation behind this library is to provide a way to test MVC actions quickly, tersely and maintainably. Most examples we find on MVC controller testing are incredibly verbose, repetitive and time-consuming to write and maintain. Given how quickly you can write controller actions and how simple they are (assuming you are following best practices and keeping them lean) the time to test them generally isn't worth it given you can glance at most of your controller actions and know they are right or wrong instantly. This library aims to make the time to implement the tests inconsequential and then the value your tests are providing is worth it. The other problem that we've noticed with most examples of controller testing is that there are a lot of magic strings being used to test view and action names; this library also aims to (where possible) utilise the type system to resolve a lot of those magic strings, thus ensuring your tests are more maintainable and require less re-work when you perform major refactoring of your code.

This library was inspired by the [MVCContrib.TestHelper](http://mvccontrib.codeplex.com/wikipage?title=TestHelper) library.

Documentation
-------------

Please see [the documentation](http://teststack.github.com/pages/fluentmvctesting.html) for installation and usage instructions.

Any questions, comments or additions?
-------------------------------------

Leave an issue on the [issues page](https://github.com/TestStack/TestStack.FluentMVCTesting/issues) or send a [pull request](https://github.com/TestStack/TestStack.FluentMVCTesting/pulls).
