using System;
using System.Collections.Generic;

namespace FS.Query.Settings.Caching
{
    public class TypeMapping
    {
        public Dictionary<Type, TypeMap> Map = DefaultTypeMap.GetMap();

        public virtual TypeMap GetDbType(Type type)
        {
            if (Map.TryGetValue(type, out TypeMap? typeMap))
                return typeMap;

            return DefaultTypeMap.GetDefaultMap(type);
        }

        public object ToSql(Type type, object? value)
        {
            if (value is null)
                return "NULL";

            var typeMap = GetDbType(type);
            var convertedValue = typeMap.ToDatabaseConversion(value);
            return convertedValue;
        }

        public object ToSqlWithCast(Type type, object? value)
        {
            if (value is null)
                return "NULL";

            var typeMap = GetDbType(type);
            var convertedValue = typeMap.ToDatabaseConversion(value);
            return $"CAST({convertedValue} AS {typeMap.DatabaseType})";
        }
    }
}
