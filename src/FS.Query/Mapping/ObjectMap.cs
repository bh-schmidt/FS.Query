using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.Query.Mapping
{
    public class ObjectMap
    {
        private string tableSchema;
        private string tableName;

        public List<PropertyMap> PropertyMaps { get; } = new List<PropertyMap>();
        public Dictionary<string, PropertyMap> PropertiesToColumns = new Dictionary<string, PropertyMap>();

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
                var trim = value.Trim("[]".ToArray());
                tableSchema = trim;

                TableFullName = GetTableFullName();
            }
        }
        public string TableName
        {
            get => tableName;
            set
            {
                var trim = value.Trim("[]".ToArray());
                tableName = trim;

                TableFullName = GetTableFullName();
            }
        }

        public string TableFullName { get; set; }

        public void Build()
        {
            PropertiesToColumns = PropertyMaps.ToDictionary(e => e.PropertyName);
        }

        private string GetTableFullName() => $"[{tableSchema}].[{TableName}]";
    }
}
