using FS.Query.Settings;
using System;
using System.Data;

namespace FS.Query.Scripts.Filters
{
    public class TableProperty : IScriptColumn
    {
        private string? columnName;
        private string? columnFullName;
        private DbType? dbType;

        public TableProperty(string tableAlias, Type tableType, string propertyName)
        {
            TableAlias = tableAlias;
            TableType = tableType;
            PropertyName = propertyName;
        }

        public string TableAlias { get; }
        public Type TableType { get; }
        public string PropertyName { get; }
        public string ColumnName => columnName ?? throw new Exception("Column not builded yet.");
        public string ColumnFullName => columnFullName ?? throw new Exception("Column not builded yet.");
        public DbType DbType => dbType ?? throw new Exception("Column not builded yet.");

        public object Build(DbSettings dbSettings)
        {
            var map = dbSettings.MapCaching.GetOrCreate(TableType, dbSettings);
            var propertyMap = map.GetProperty(PropertyName);

            if (propertyMap is null)
                throw new Exception("An error ocurred getting the property informed.");

            dbType = propertyMap.DbType;
            columnName = propertyMap.ColumnName;
            columnFullName = $"[{TableAlias}].{propertyMap.TreatedColumnName}";
            return ColumnFullName;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not TableProperty tableProperty)
                return false;

            return
                TableAlias == tableProperty.TableAlias &&
                TableType == tableProperty.TableType &&
                PropertyName == tableProperty.PropertyName;
        }

        public string GetColumnName(DbSettings dbSettings)
        {
            var map = dbSettings.MapCaching.GetOrCreate(TableType, dbSettings);
            var propertyMap = map.GetProperty(PropertyName);

            if (propertyMap is null)
                return PropertyName;

            return propertyMap.ColumnName;
        }

        public override int GetHashCode() => HashCode.Combine(TableAlias, TableType, PropertyName);
    }
}
