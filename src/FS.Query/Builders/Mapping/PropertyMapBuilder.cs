using FS.Query.Settings.Mapping;
using System;

namespace FS.Query.Builders.Mapping
{
    public class PropertyMapBuilder<TProperty> : IPropertyMapBuilder<TProperty>
    {
        public PropertyMap PropertyMap { get; }

        public PropertyMapBuilder(PropertyMap propertyMap)
        {
            PropertyMap = propertyMap;
        }

        public PropertyMapBuilder(string name, Type propertyType)
        {
            PropertyMap = new PropertyMap(name, propertyType);
        }

        public PropertyMapBuilder<TProperty> WithName(string databaseName)
        {
            PropertyMap.ColumnName = databaseName;
            return this;
        }
    }
}
