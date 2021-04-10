namespace FS.Query.Scripts.SelectionScripts.Orders
{
    public class ColumnOrder
    {
        public ColumnOrder(IScriptColumn scriptColumn, bool descending)
        {
            ScriptColumn = scriptColumn;
            Descending = descending;
        }

        public virtual IScriptColumn ScriptColumn { get; }
        public virtual bool Descending { get; }
    }
}
