using FS.Query.Builders.Mapping;
using FS.Query.Helpers.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FS.Query.Settings.Mapping
{
    public class TableMap<TType> : ITableMap
        where TType : new()
    {
        public ObjectMap ObjectMap { get; } = new ObjectMap(typeof(TType));

        public TableMap<TType> TableName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            ObjectMap.TableName = name;

            return this;
        }

        public TableMap<TType> TableSchema(string schema)
        {
            if (string.IsNullOrWhiteSpace(schema))
                throw new ArgumentException($"'{nameof(schema)}' cannot be null or whitespace.", nameof(schema));

            ObjectMap.TableSchema = schema;

            return this;
        }

        public IPropertyMapBuilder<TProperty> Property<TProperty>(Expression<Func<TType, TProperty?>> getProperty)
        {
            var propertyInfo = getProperty.GetPropertyInfo();

            PropertyMapBuilder<TProperty> builder;

            PropertyMap? propertyMap = ObjectMap.PropertyMaps.FirstOrDefault(e => e.PropertyName == propertyInfo.Name);
            if (propertyMap is null)
            {
                builder = new PropertyMapBuilder<TProperty>(propertyInfo.Name, propertyInfo.PropertyType);
                ObjectMap.PropertyMaps.Add(builder.PropertyMap);
                return builder;
            }

            builder = new PropertyMapBuilder<TProperty>(propertyMap);

            return builder;
        }
    }
}
