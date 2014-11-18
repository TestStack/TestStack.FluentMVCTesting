﻿using System.Linq.Expressions;

namespace TestStack.FluentMVCTesting.Internal
{
    internal class ExpressionInspector
    {
        internal string Inspect(LambdaExpression expression) => 
            expression.ToString()
                .Replace(" OrElse ", " || ")
                .Replace(" AndAlso ", " && ")
                .Replace(" Or ", " | ")
                .Replace(" And ", " & ");
    }
}