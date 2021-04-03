using FS.Query.Scripts.Filters.Comparables;

namespace FS.Query.Scripts
{
    public interface IScriptColumn : ISqlComparable
    {
        public string TableAlias { get; }
        public string ColumnName { get; }
        public string ColumnFullName { get; }
    }
}
