using System;
using System.Data;

namespace FS.Query.Settings.Mapping
{
    public class PropertyMap
    {
        private string columnName = "";
        private DbType? dbType;

        public PropertyMap(string name, Type propertyType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            PropertyName = name;
            PropertyType = propertyType;
            ColumnName = name;
        }

        public string PropertyName { get; set; }
        public Type PropertyType { get; }
        public string ColumnName
        {
            get => columnName;
            set
            {
                columnName = value ?? PropertyName;
                TreatedColumnName = $"[{columnName}]";
            }
        }
        public string TreatedColumnName { get; private set; } = "";
        public DbType DbType { get => dbType ?? throw new Exception("PropertyMap not builded."); set => dbType = value; }

        public virtual void Build(DbSettings dbSettings)
        {
            dbType ??= dbSettings.TypeMapping.GetTypeMap(PropertyType).DbType;
        }
    }
}
