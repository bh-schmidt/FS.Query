using FS.Query.Settings;
using System;

namespace FS.Query.Scripts.Columns
{
    public class AliasTableColumn : TableColumn, IAliasColumn
    {
        private string? columnFullName;

        public AliasTableColumn(string tableAlias, Type tableType, string propertyName) : base(tableType, propertyName)
        {
            TableAlias = tableAlias;
        }

        public string TableAlias { get; }
        public string ColumnFullName => columnFullName ?? throw new Exception("Column not builded yet.");

        public override object Build(DbSettings dbSettings)
        {
            BuildInternal(dbSettings);
            return TreatedColumnName;
        }

        public object BuildWithAlias(DbSettings dbSettings)
        {
            BuildInternal(dbSettings);
            return ColumnFullName;
        }

        protected override void BuildInternal(DbSettings dbSettings, Settings.Mapping.PropertyMap? propertyMap = null)
        {
            var map = dbSettings.MapCaching.GetOrCreate(TableType, dbSettings);
            propertyMap = map.GetProperty(PropertyName);

            base.BuildInternal(dbSettings, propertyMap);
            columnFullName = $"[{TableAlias}].{propertyMap!.TreatedColumnName}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not AliasTableColumn tableProperty)
                return false;

            return
                TableAlias == tableProperty.TableAlias &&
                TableType == tableProperty.TableType &&
                PropertyName == tableProperty.PropertyName;
        }

        public override int GetHashCode() => HashCode.Combine(TableAlias, TableType, PropertyName);
    }
}
