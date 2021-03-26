using FS.Query.Mapping;
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

        public PropertyMapBuilder(string name)
        {
            PropertyMap = new PropertyMap(typeof(TProperty), name);
        }

        public PropertyMapBuilder<TProperty> WithName(string databaseName)
        {
            PropertyMap.ColumnName = databaseName;
            return this;
        }
    }
}
