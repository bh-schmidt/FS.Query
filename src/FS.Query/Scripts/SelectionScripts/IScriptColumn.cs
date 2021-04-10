using FS.Query.Scripts.SelectionScripts.Filters.Comparables;

namespace FS.Query.Scripts.SelectionScripts
{
    public interface IScriptColumn : ISqlComparable
    {
        public string TableAlias { get; }
        public string ColumnName { get; }
        public string ColumnFullName { get; }
    }
}
