using System.Linq.Expressions;
using ExpressionToString;

namespace TestStack.FluentMVCTesting.Internal
{
    internal class ExpressionInspector
    {
        internal string Inspect(LambdaExpression expression)
        {
            return ExpressionStringBuilder.ToString(expression);
        }
    }
}