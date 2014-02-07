using System;

namespace TestStack.FluentMVCTesting
{

    public class ActionResultAssertionException : Exception
    {
        public ActionResultAssertionException(string message) : base(message) { }
    }

    public class ViewResultModelAssertionException : Exception
    {
        public ViewResultModelAssertionException(string message) : base(message) { }
    }

    public class ModelErrorAssertionException : Exception
    {
        public ModelErrorAssertionException(string message) : base(message) { }
    }

    public class InvalidControllerActionException : Exception
    {
        public InvalidControllerActionException(string message) : base(message) { }
    }

    public class InvalidRouteValueException : Exception
    {
        public InvalidRouteValueException(string message) : base(message) { }
    }

    public class MissingRouteValueException : Exception
    {
        public MissingRouteValueException(string message) : base(message) { }
    }

    public class ValueTypeMismatchException : Exception
    {
        public ValueTypeMismatchException(string message) : base(message) { }
    }
}
