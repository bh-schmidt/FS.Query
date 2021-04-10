using FS.Query.Scripts.SelectionScripts;
using FS.Query.Settings;
using System.Collections.Generic;

namespace FS.Query.Scripts.InsertionScripts.Entries
{
    public abstract class Entry
    {
        public abstract object Build(DbSettings dbSettings, IEnumerable<IScriptColumn> scriptColumns);
    }
}
