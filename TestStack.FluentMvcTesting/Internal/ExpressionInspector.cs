using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace TestStack.FluentMVCTesting.Internal
{
    internal class ExpressionInspector
    {
        internal string Inspect(LambdaExpression expression)
        {
            return Regex.Replace(expression.ToString(), "[()]", "")
                .Replace(" OrElse ", " || ");
        }
    }
}