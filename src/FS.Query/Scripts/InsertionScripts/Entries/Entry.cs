using FS.Query.Scripts.Columns;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.Query.Scripts.InsertionScripts.Entries
{
    public abstract class Entry
    {
        public Entry(IEnumerable<IColumn> scriptColumns)
        {
            if (scriptColumns is null || !scriptColumns.Any())
                throw new ArgumentException("The script columns can't be null or empty.");

            ScriptColumns = scriptColumns;
        }

        public IEnumerable<IColumn> ScriptColumns { get; }

        public abstract object Build(DbSettings dbSettings);
    }
}
