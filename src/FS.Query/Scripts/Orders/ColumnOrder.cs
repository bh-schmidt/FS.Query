namespace FS.Query.Scripts.Orders
{
    public struct ColumnOrder
    {
        public ColumnOrder(IScriptColumn scriptColumn, bool descending)
        {
            ScriptColumn = scriptColumn;
            Descending = descending;
        }

        public IScriptColumn ScriptColumn { get; }
        public bool Descending { get; }
    }
}
