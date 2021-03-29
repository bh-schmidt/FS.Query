using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FS.Query.Helpers.Extensions
{
    public static class ExpressionExtensions
    {
        private const byte defaultCapacity = 4;

        internal static PropertyInfo GetPropertyInfo<TType, TProperty>(this Expression<Func<TType, TProperty>> expression)
        {
            var unaryExpression = expression.Body as UnaryExpression;
            var memberExpression = expression.Body as MemberExpression ?? unaryExpression?.Operand as MemberExpression;
            if (memberExpression is null)
                throw new ArgumentException($"{nameof(expression)} refers to a method, not a property.");

            return GetPropertyInfo(memberExpression);
        }

        internal static List<PropertyInfo> GetPropertyInfos<TType, TProperty>(this Expression<Func<TType, TProperty>> expression)
        {
            var propertyInfos = new List<PropertyInfo>(defaultCapacity);

            var unaryExpression = expression.Body as UnaryExpression;
            var memberExpression = expression.Body as MemberExpression ?? unaryExpression?.Operand as MemberExpression;
            if (memberExpression is null)
                throw new ArgumentException($"{nameof(expression)} refers to a method, not a property.");

            GetPropertyInfos(memberExpression, propertyInfos);

            return propertyInfos;
        }

        internal static PropertyInfo GetPropertyInfo(this MemberExpression memberExpression)
        {
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo is null)
                throw new ArgumentException($"The expression refers to a field, not a property.");

            if (memberExpression.Expression is MemberExpression insideMemberExpression)
                return GetPropertyInfo(insideMemberExpression);

            return propertyInfo;
        }

        internal static void GetPropertyInfos(this MemberExpression memberExpression, List<PropertyInfo> propertyInfos)
        {
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo is null)
                throw new ArgumentException($"The expression refers to a field, not a property.");

            propertyInfos.Add(propertyInfo);

            if (memberExpression.Expression is MemberExpression insideMemberExpression)
                GetPropertyInfos(insideMemberExpression, propertyInfos);
        }
    }
}
