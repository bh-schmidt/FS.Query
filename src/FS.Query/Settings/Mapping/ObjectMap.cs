using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.Query.Settings.Mapping
{
    public class ObjectMap
    {
        private string tableSchema;
        private string tableName;
        private Dictionary<string, PropertyMap>? propertiesToColumns;
        private Dictionary<string, PropertyMap>? columnsToProperties;

        public List<PropertyMap> PropertyMaps { get; } = new List<PropertyMap>();

        public ObjectMap(Type type)
        {
            Type = type;
            tableSchema = "dbo";
            tableName = type.Name;
            TableFullName = GetTableFullName();
        }

        public Type Type { get; }
        public string TableSchema
        {
            get => tableSchema;
            set
            {
                var trim = value.Trim("[] ".ToArray());
                tableSchema = trim;

                TableFullName = GetTableFullName();
            }
        }
        public string TableName
        {
            get => tableName;
            set
            {
                var trim = value.Trim("[] ".ToArray());
                tableName = trim;

                TableFullName = GetTableFullName();
            }
        }

        public string TableFullName { get; private set; }

        public void Build(DbSettings dbSettings)
        {
            propertiesToColumns = PropertyMaps.ToDictionary(e => e.PropertyName);
            columnsToProperties = PropertyMaps.ToDictionary(e => e.ColumnName, StringComparer.OrdinalIgnoreCase);

            foreach (var property in PropertyMaps)
                property.Build(dbSettings);
        }

        public PropertyMap? GetProperty(string propertyName) => propertiesToColumns is not null ? propertiesToColumns.GetValueOrDefault(propertyName) : throw new Exception("The object map was not builded.");
        public PropertyMap? GetColumn(string propertyName) => columnsToProperties is not null ? columnsToProperties.GetValueOrDefault(propertyName) : throw new Exception("The object map was not builded.");

        private string GetTableFullName() => $"[{tableSchema}].[{TableName}]";
    }
}
