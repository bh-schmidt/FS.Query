using FS.Query.Helpers.Extensions;
using System;
using System.Text;

namespace FS.Query.Scripts.SelectionScripts
{
    public static class ToDatabaseConversion
    {
        public static readonly Func<object, object> DefaultToDatabaseConversion = 
            value => new StringBuilder()
                .Append(value)
                .Replace("'", "''")
                .SurroundByQuotes();

        public static readonly Func<object, object> BoolToDatabaseConversion = 
            value => (bool)value == true ? 1 : 0;

        public static readonly Func<object, object> DateTimeToDatabaseConversion = 
            value => $"'{(DateTime)value:yyyy-MM-dd hh:mm:ss.fffffff zzz}'";

        public static readonly Func<object, object> DateTimeOffsetToDatabaseConversion =
           value => $"'{(DateTimeOffset)value:yyyy-MM-dd hh:mm:ss.fffffff zzz}'";

        public static readonly Func<object, object> TimeSpanToDatabaseConversion = 
            value => ((TimeSpan)value).Ticks;
    }
}
