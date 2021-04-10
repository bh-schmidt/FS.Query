using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Sources;
using System.Collections.Generic;

namespace FS.Query.Scripts.InsertionScripts
{
    public class InsertionScript
    {
        private LinkedList<IScriptColumn>? columns;

        public InsertionScript(Table table)
        {
            Table = table;
        }

        public Table Table { get; }
        public LinkedList<IScriptColumn> Columns => columns ??= new();
    }
}
