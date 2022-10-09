using System;

namespace FS.Query.Settings.Conversions
{
    public static class FromDatabaseConversion
    {
        public static readonly Func<Type, object, object> Default =
            (type, value) => Convert.ChangeType(value, type);

        public static readonly Func<Type, object, object> ToEnum =
            (type, value) => Enum.ToObject(type, value);
    }
}
