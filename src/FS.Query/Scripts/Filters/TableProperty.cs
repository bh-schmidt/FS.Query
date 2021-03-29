using FS.Query.Settings;
using System;

namespace FS.Query.Scripts.Filters
{
    public class TableProperty : IScriptColumn
    {
        private string? treatedTableAlias;

        public TableProperty(string tableAlias, Type tableType, string propertyName)
        {
            TableAlias = tableAlias;
            TableType = tableType;
            PropertyName = propertyName;
        }

        public string TableAlias { get; }
        public string TreatedTableAlias { get => treatedTableAlias ??= $"[{TableAlias}]"; }
        public Type TableType { get; }
        public string PropertyName { get; }

        public object Build(DbSettings dbSettings)
        {
            var map = dbSettings.MapCaching.GetOrCreate(TableType);
            var propertyMap = map.GetProperty(PropertyName);

            if (propertyMap is null)
                return $"{TreatedTableAlias}.[{PropertyName}]";

            return $"{TreatedTableAlias}.{propertyMap.TreatedColumnName}";
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

        public override int GetHashCode() => HashCode.Combine(TableAlias, TableType, PropertyName);
    }
}
