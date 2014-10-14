using NUnit.Framework;
using System;
using System.Linq.Expressions;
using TestStack.FluentMVCTesting.Internal;

namespace TestStack.FluentMVCTesting.Tests.Internal
{
    [TestFixture]
    public class ExpressionInspectorShould
    {
        [Test]
        public void Correctly_parse_equality_comparison_with_string_operands()
        {
            Expression<Func<string, bool>> func = text => text == "any";
            ExpressionInspector sut = new ExpressionInspector();
            var actual = sut.Inspect(func);
            Assert.AreEqual("text => text == \"any\"", actual);
        }

        [Test]
        public void Correctly_parse_equality_comparison_with_int_operands()
        {
            Expression<Func<int, bool>> func = number => number == 5;
            ExpressionInspector sut = new ExpressionInspector();
            var actual = sut.Inspect(func);
            Assert.AreEqual("number => number == 5", actual);
        }

        [Test]
        public void Correctly_parse_inequality_comparison()
        {
            Expression<Func<int, bool>> func = number => number != 5;
            ExpressionInspector sut = new ExpressionInspector();
            var actual = sut.Inspect(func);
            Assert.AreEqual("number => number != 5", actual);
        }

        [Test]
        public void Correctly_parse_equality_comparison_with_captured_constant_operand()
        {
            const int Number = 5;
            Expression<Func<int, bool>> func = number => number == Number;
            ExpressionInspector sut = new ExpressionInspector();
            var actual = sut.Inspect(func);
            Assert.AreEqual("number => number == " + Number, actual);
        }

        [Test]
        public void Correctly_parse_relational_comparison()
        {
            Expression<Func<int, bool>> func = number => number < 5;
            ExpressionInspector sut = new ExpressionInspector();
            var actual = sut.Inspect(func);
            Assert.AreEqual("number => number < 5", actual);
        }
    }
}