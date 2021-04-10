using FS.Query.Settings;
using FS.Query.Settings.Mapping;
using System;

namespace FS.Query.Scripts.SelectionScripts.Serializations
{
    public struct ColumnSerialization
    {
        public ColumnSerialization(Type type, int index, IScriptColumn column, string[]? propertyHierarchy, DbSettings dbSettings)
        {
            Type = type;
            Index = index;
            PropertyHierarchy = propertyHierarchy;
            PropertyMap = GetPropertyMap(Type, propertyHierarchy, column, dbSettings);
            BuildedHierarchy = propertyHierarchy is null ? string.Empty : string.Join('.', propertyHierarchy);
        }

        public Type Type { get; }
        public int Index { get; }
        public string[]? PropertyHierarchy { get; }
        public PropertyMap? PropertyMap { get; }
        public string BuildedHierarchy { get; }

        public object GetByHierarchy(object source)
        {
            if (PropertyHierarchy is null)
                return source;

            object nextObject = source;
            var nextType = Type;
            foreach (var property in PropertyHierarchy)
            {
                var propertyInfo = nextType.GetProperty(property);
                if (propertyInfo is null)
                    throw new Exception("The property hierarchy is invalid.");

                var value = propertyInfo.GetValue(nextObject);
                if (value is null)
                {
                    value = Activator.CreateInstance(propertyInfo.PropertyType);
                    propertyInfo.SetValue(nextObject, value);
                }

                if (value is null)
                    throw new Exception($"The property {propertyInfo.Name} couldn't be initialized.");

                nextObject = value;
                nextType = propertyInfo.PropertyType;
            }

            return nextObject;
        }

        private static PropertyMap? GetPropertyMap(Type sourceType, string[]? propertyHierarchy, IScriptColumn column, DbSettings dbSettings)
        {
            ObjectMap map;
            if (propertyHierarchy is null)
            {
                map = dbSettings.MapCaching.GetOrCreate(sourceType, dbSettings);
                return map.GetColumn(column.ColumnName);
            }

            var nextType = sourceType;
            foreach (var property in propertyHierarchy)
            {
                var propertyInfo = nextType.GetProperty(property);
                if (propertyInfo is null)
                    throw new Exception("The property hierarchy is invalid.");

                nextType = propertyInfo.PropertyType;
            }

            map = dbSettings.MapCaching.GetOrCreate(nextType, dbSettings);
            return map.GetColumn(column.ColumnName);
        }
    }
}
