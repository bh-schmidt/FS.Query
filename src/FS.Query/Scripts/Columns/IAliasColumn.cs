using FS.Query.Scripts.Columns;
namespace FS.Query.Scripts.Columns
{
    public interface IAliasColumn : IColumn, ISqlComparable
    {
        public string TableAlias { get; }
        public string ColumnFullName { get; }
    }
}
