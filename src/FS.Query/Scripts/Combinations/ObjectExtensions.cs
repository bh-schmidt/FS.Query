using System;
using System.Linq.Expressions;

namespace FS.Query.Scripts.Combinations
{
    public static class ObjectExtensions
    {
        internal static Type GetObjectType(this object? source)
        {
            if (source is not null)
                return source.GetType();

            Expression<Func<object?>> expr = () => source;
            var memberExpression = (MemberExpression) ((UnaryExpression)expr.Body).Operand;
            return memberExpression.GetType();
        }
    }
}
