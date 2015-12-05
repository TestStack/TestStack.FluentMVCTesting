using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace Xania.AspNet.Simulator
{
    internal class LinqActionDescriptor : ReflectedActionDescriptor
    {
        private LinqActionDescriptor(MethodCallExpression methodCallExpression, ReflectedControllerDescriptor controllerDescriptor)
            : base(methodCallExpression.Method, methodCallExpression.Method.Name, controllerDescriptor)
        {
        }

        public static LinqActionDescriptor Create<TController>(Expression<Func<TController, object>> actionExpression)
            where TController: ControllerBase
        {
            var methodCallExpression = GetMethodCallExpression(actionExpression.Body);

            return new LinqActionDescriptor(methodCallExpression, new ReflectedControllerDescriptor(typeof(TController)));
        }

        private static MethodCallExpression GetMethodCallExpression(Expression actionBody)
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

        public static LinqActionDescriptor Create<TController>(Expression<Action<TController>> actionExpression)
            where TController: ControllerBase
        {
            var methodCallExpression = (MethodCallExpression)actionExpression.Body;

            return new LinqActionDescriptor(methodCallExpression, new ReflectedControllerDescriptor(typeof(TController)));
        }
    }
}