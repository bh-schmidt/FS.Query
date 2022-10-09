using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.Columns;
using FS.Query.Scripts.InsertionScripts;
using FS.Query.Scripts.InsertionScripts.Entries;
using FS.Query.Scripts.SelectionScripts.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FS.Query.Builders.Scripts.InsertionScripts
{
    public class InsertionBuilder<TTable>
    {
        private readonly InsertionScript insertionScript;
        private readonly DbManager dbManager;

        public InsertionBuilder(Table table, DbManager dbManager)
        {
            insertionScript = new InsertionScript(table);
            this.dbManager = dbManager;
        }

        public InsertionBuilder<TTable> Columns(params Expression<Func<TTable, object?>>[] columns)
        {
            if (columns is null)
                return this;

            var tableType = typeof(TTable);

            foreach (var column in columns)
            {
                var property = column.GetPropertyInfo();
                var tableColumn = new TableColumn(tableType, property.Name);
                insertionScript.Columns.AddLast(tableColumn);
            }

            return this;
        }

        public InsertionBuilder<TTable> Value(TTable value)
        {
            var entry = new EnumerableEntry(
                typeof(TTable),
                new object[] { value! },
                dbManager.DbSettings,
                insertionScript.Columns);

            insertionScript.Entry = entry;
            return this;
        }

        public InsertionBuilder<TTable> Values(IEnumerable<TTable> values)
        {
            var entry = new EnumerableEntry(
                typeof(TTable),
                values.OfType<object>(),
                dbManager.DbSettings,
                insertionScript.Columns);

            insertionScript.Entry = entry;
            return this;
        }
    }
}
