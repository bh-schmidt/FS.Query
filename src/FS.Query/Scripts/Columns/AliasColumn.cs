using FS.Query.Settings;
using FS.Query.Scripts.Columns;

namespace FS.Query.Scripts.Columns
{
    public class AliasColumn : Column, IAliasColumn
    {
        private string? columnFullName;

        public AliasColumn(string tableAlias, string columnName) : base(columnName)
        {
            TableAlias = tableAlias;
        }

        public string TableAlias { get; }
        public string ColumnFullName => columnFullName ??= $"[{TableAlias}].[{ColumnName}]";

        public object BuildWithAlias(DbSettings dbSettings) =>
            ColumnFullName;
    }
}
