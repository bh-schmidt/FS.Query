using System;
using System.Collections.Generic;

namespace FS.Query.Settings.Conversions
{
    public class TypeMapping
    {
        private static readonly Type EnumType = typeof(Enum);
        public Dictionary<Type, TypeMap> Map = DefaultTypeMap.GetMap();

        public virtual TypeMap GetTypeMap(Type type)
        {
            if (Map.TryGetValue(type, out TypeMap? typeMap))
                return typeMap;

            if(EnumType.IsAssignableFrom(type))
                return DefaultTypeMap.GetDefaultEnumMap(type);

            return DefaultTypeMap.GetDefaultMap(type);
        }

        public object MapToSql(Type type, object? value)
        {
            var typeMap = GetTypeMap(type);
            var convertedValue = typeMap.ToDatabaseConversion(value);
            return convertedValue;
        }
    }
}
