# Version 3.0.0

## ShouldRedirectTo Method

It used to be the case that when you invoked `ShouldRedirectTo<TController>` where `TController` is the same type as the `Controller` under test like this:

	var sut = new HomeController();
	sut.WithCallTo(c => c.Index())
	    .ShouldRedirectTo<HomeController>(c => c.Index());
 
  an `ActionResultAssertionException` would be thrown. 

An exception is no longer thrown.

### Reason

There is no reason why a test like this one should fail.

You can read the original problem specification and discussion  [here](https://github.com/TestStack/TestStack.FluentMVCTesting/issues/47).


### Fix

If your project has been impacted by this particular breaking change, you might consider reevaluate the correctness of the affected tests. 

## Error Message Quotes

Some error messages surrounded actual values in double quotes. Others surrounded the values in single quotes. In version 3.0.0 *all* values are surrounded in single quotes.

### Reason

Consistency.

### Fix

Amend any affected tests to expect single quotes instead of double quotes.

## Error Message Lambda Expression

In error messages, lambda expressions arguments are now surrounded in a pair of parentheses. For example:

	... to pass the given condition (model => (model.Property1 != null))

will now look like this:

	... to pass the given condition ((model) => (model.Property1 != null))

As you can see, the argument called `model` is now surrounded in parentheses.

###Reason

FluentMVCTesting now uses [ExpressionToString](https://github.com/JakeGinnivan/ExpressionToString) to humanize expression trees. ExpressionToString surrounds arguments in parentheses.

###Fix

Amend any affected tests to expect lambda expression arguments to be surrounded in parentheses.

# Version 2.0.0

## ShouldRenderFileStream Method

The following overload of the `ShouldRenderFileStream` method has been *replaced*:

    public FileStreamResult ShouldRenderFileStream(string contentType = null)

We place emphasis on the word "replace" because it is important to note that this overload has not been removed but replaced - you will not encounter a compile-time error if you upgrade, but you will encounter a logical error when you run your existing test.

### Reason

The aforementioned overload has been replaced in order to enable an overload that takes a stream for comparison in a way that is consistent with the existing convention.

### Fix

Use a [named argument](http://msdn.microsoft.com/en-gb/library/dd264739.aspx).

As where you would have previously done this:

    ShouldRenderFileStream("application/json");
 
You must now do this: 

    ShouldRenderFileStream(contentType: "application/json");
    
    
## ShouldRenderFileMethod

The `ShouldRenderFile` method has been removed.

### Reason

The `ShouldRenderFile` method was ambiguous because it had the possibility to be interperted to test for a `FileResult` when in fact, it tested for a `FileContentResult`. 

It is for this reason that we introduced two unequivocal methods namely, `ShouldRenderAnyFile` and `ShouldRenderFileContents`.

### Fix

Use the `ShouldRenderFileContents` method instead: 

    ShouldRenderAnyFile()
    ShouldRenderAnyFile(contentType: "application/json")
    