using FS.Query.Helpers.Extensions;
using System;
using System.Text;

namespace FS.Query.Settings.Conversions
{
    public static class ToDatabaseConversion
    {
        public static readonly Func<object, object> Default =
            value => new StringBuilder()
                .Append(value)
                .Replace("'", "''")
                .SurroundByQuotes();

        public static readonly Func<object, object> FromNumber =
            value => value;

        public static readonly Func<object, object> FromBool =
            value => (bool)value == true ? 1 : 0;

        public static readonly Func<object, object> FromEnum =
            value => $"{value:d}";

        public static readonly Func<object, object> FromDateTime =
            value => $"'{(DateTime)value:yyyy-MM-dd hh:mm:ss.fffffff zzz}'";

        public static readonly Func<object, object> FromDateTimeOff =
           value => $"'{(DateTimeOffset)value:yyyy-MM-dd hh:mm:ss.fffffff zzz}'";

        public static readonly Func<object, object> FromTimeSpan =
            value => ((TimeSpan)value).Ticks;
    }
}
