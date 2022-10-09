using FS.Query.Scripts.Columns;

namespace FS.Query.Scripts.SelectionScripts.Orders
{
    public class ColumnOrder
    {
        public ColumnOrder(IAliasColumn scriptColumn, bool descending)
        {
            ScriptColumn = scriptColumn;
            Descending = descending;
        }

        public virtual IAliasColumn ScriptColumn { get; }
        public virtual bool Descending { get; }
    }
}
