using System;
using System.Text;

namespace FS.Query.Scripts
{
    public static class SqlConversion
    {
        private static readonly Type BoolType = typeof(bool);
        private static readonly Type NullableBoolType = typeof(bool?);
        private static readonly Type GuidType = typeof(Guid);
        private static readonly Type NullableGuidType = typeof(Guid?);
        private static readonly Type DateTimeType = typeof(Guid?);
        private static readonly Type NullableDateTimeType = typeof(Guid?);

        public static object ToSql(Type type, object? value)
        {
            if (value is null)
                return "NULL";

            if (type == BoolType || type == NullableBoolType)
                return (bool)value == true ? 1 : 0;

            else if (type == GuidType || type == NullableGuidType)
                return $"CAST('{value}' AS UNIQUEIDENTIFIER)";

            else if (type == DateTimeType || type == NullableDateTimeType)
                return $"'{(DateTime)value:yyyy-MM-dd hh:mm:ss}'";

            return new StringBuilder()
                .Append(value)
                .Replace("'", "''")
                .SurroundByQuotes();
        }

        private static StringBuilder SurroundByQuotes(this object value) =>
            new StringBuilder()
                .Append('\'')
                .Append(value)
                .Append('\'');
    }
}
