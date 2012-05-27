using System;

namespace TerseControllerTesting
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
}
