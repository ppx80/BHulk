using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BHulk.Util
{
    internal static class LambdaExpressionExtensions
    {
        internal static MemberInfo GetMember(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member;
                case ExpressionType.Convert:
                    return GetMember(((UnaryExpression)expression).Operand);
                default:
                    throw new NotSupportedException(expression.NodeType.ToString());
            }
        }
    }
}
