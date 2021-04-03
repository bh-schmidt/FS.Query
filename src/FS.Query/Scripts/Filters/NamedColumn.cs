using FS.Query.Settings;
using System;

namespace FS.Query.Scripts.Filters
{
    class NamedColumn : IScriptColumn
    {
        private readonly string? tableAlias;
        private string? columnFullName;

        public NamedColumn(string columnName)
        {
            ColumnName = columnName;
        }

        public NamedColumn(string tableAlias, string columnName)
        {
            this.tableAlias = tableAlias;
            ColumnName = columnName;
        }

        public string ColumnName { get; }
        public string TableAlias => tableAlias ?? throw new Exception("This column doesn't received a tableAlias.");
        public string ColumnFullName => columnFullName ?? throw new Exception("Column not builded yet.");

        public object Build(DbSettings dbSettings)
        {
            columnFullName = $"[{TableAlias}].[{ColumnName}]";
            return columnFullName;
        }
    }
}
