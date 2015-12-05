using System;
using System.Linq.Expressions;

namespace TestStack.FluentMVCTesting.MvcPipeline
{
    public static class Reflector
    {
        public static MethodCallExpression GetMethodCallExpression(Expression actionBody)
        {
            var methodCallExpression = actionBody as MethodCallExpression;
            if (methodCallExpression != null)
                return methodCallExpression;

            if (!(actionBody is UnaryExpression))
                throw new InvalidOperationException("Unable to reduce expression to MethodCallExpression");

            var unaryExpr = actionBody as UnaryExpression;
            if (unaryExpr.NodeType == ExpressionType.Convert)
                return GetMethodCallExpression(unaryExpr.Operand);

            throw new InvalidOperationException("Unable to reduce expression to MethodCallExpression");
        }
    }
}