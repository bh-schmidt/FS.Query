using FS.Query.Settings;
using FS.Query.Scripts.Columns;

namespace FS.Query.Scripts.Columns
{
    public class Column : IColumn
    {
        private string? treatedColumnName;

        public Column(string columnName)
        {
            ColumnName = columnName;
        }

        public virtual string ColumnName { get; }

        public virtual string TreatedColumnName => treatedColumnName ??= $"[{ColumnName}]";

        public virtual object Build(DbSettings dbSettings) =>
            TreatedColumnName;
    }
}
