using FS.Query.Settings;
using FS.Query.Settings.Mapping;
using System;
using System.Data;

namespace FS.Query.Scripts.Columns
{
    public class TableColumn : IColumn
    {
        private string? columnName;
        private string? treatedColumnName;
        private DbType? dbType;

        public TableColumn(Type tableType, string propertyName)
        {
            TableType = tableType;
            PropertyName = propertyName;
        }

        public virtual Type TableType { get; }
        public virtual string PropertyName { get; }
        public virtual string ColumnName => columnName ?? throw new Exception("Column not builded yet.");
        public virtual string TreatedColumnName => treatedColumnName ?? throw new Exception("Column not builded yet.");
        public virtual DbType DbType => dbType ?? throw new Exception("Column not builded yet.");

        public virtual object Build(DbSettings dbSettings)
        {
            BuildInternal(dbSettings);
            return TreatedColumnName;
        }

        protected virtual void BuildInternal(DbSettings dbSettings, PropertyMap? propertyMap = null)
        {
            if (propertyMap is null)
            {
                var map = dbSettings.MapCaching.GetOrCreate(TableType, dbSettings);
                propertyMap = map.GetProperty(PropertyName);
            }

            if (propertyMap is null)
                throw new Exception("An error ocurred getting the property informed.");

            dbType = propertyMap.DbType;
            columnName = propertyMap.ColumnName;
            treatedColumnName = $"[{ColumnName}]";
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not AliasTableColumn tableProperty)
                return false;

            return
                TableType == tableProperty.TableType &&
                PropertyName == tableProperty.PropertyName;
        }

        public override int GetHashCode() => HashCode.Combine(TableType, PropertyName);
    }
}
