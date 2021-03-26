using System;
using System.Text;

namespace FS.Query.Factory
{
    public static class SqlConversion
    {
        private static readonly Type BoolType = typeof(bool);
        private static readonly Type NullableBoolType = typeof(bool?);
        private static readonly Type GuidType = typeof(Guid);
        private static readonly Type NullableGuidType = typeof(Guid?);
        private static readonly Type DateTimeType = typeof(Guid?);
        private static readonly Type NullableDateTimeType = typeof(Guid?);

        public static StringBuilder ToSql(Type type, object? value)
        {
            StringBuilder stringBuilder = new();

            if(value is null)
                return stringBuilder.Append("NULL");

            if (type == BoolType || type == NullableBoolType)
                return stringBuilder
                    .Append((bool)value == true ? 1 : 0);

            else if (type == GuidType || type == NullableGuidType)
                return stringBuilder
                    .Append('\'')
                    .Append("CAST('")
                    .Append(value.ToString())
                    .Append("' AS UNIQUEIDENTIFIER)")
                    .Append('\'');

            else if (type == DateTimeType || type == NullableDateTimeType)
                return stringBuilder
                    .Append('\'')
                    .Append(((DateTime)value).ToString("yyyy-MM-dd hh:mm:ss"))
                    .Append('\'');

            else
                stringBuilder
                    .Append('\'')
                    .Append(value.ToString())
                    .Append('\'');

            return stringBuilder;
        }
    }
}
