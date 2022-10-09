using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Scripts.SelectionScripts.Selects;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FS.Query.Scripts.Columns;

namespace FS.Query.Scripts.SelectionScripts.Serializations
{
    public struct ScriptSerialization
    {
        private readonly ColumnSerialization[] columnSerializations;

        public ScriptSerialization(Type type, Select[] selects, IDataReader reader, DbSettings dbSettings)
        {
            columnSerializations = GetSerializations(type, selects, reader, dbSettings).OrderBy(e => e.BuildedHierarchy).ToArray();
        }

        public void Serialize(IDataReader reader, object source)
        {
            var hierachyObject = source;
            string? lastHierarchy = null;

            foreach (var columnSerialization in columnSerializations)
            {
                if (columnSerialization.PropertyMap is null)
                    continue;

                var value = reader.GetValue(columnSerialization.Index);
                if (value == DBNull.Value)
                    continue;

                if (lastHierarchy != columnSerialization.BuildedHierarchy)
                {
                    hierachyObject = columnSerialization.GetByHierarchy(source);
                    lastHierarchy = columnSerialization.BuildedHierarchy;
                }

                columnSerialization.SetValue(hierachyObject, value);
            }
        }

        private static IEnumerable<ColumnSerialization> GetSerializations(
            Type type,
            Select[] selects,
            IDataReader reader,
            DbSettings dbSettings)
        {
            var columns = selects.SelectMany(e => e.Columns).OfType<IColumn>().ToArray();
            var addedCollumns = new HashSet<IColumn>(columns.Length);
            LinkedList<IColumn> newColumns = new();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var column = columns.FirstOrDefault(e => string.Equals(e.ColumnName, columnName, StringComparison.OrdinalIgnoreCase) && !addedCollumns.Contains(e));

                if (column is null)
                {
                    column = new Column(columnName);
                    newColumns.AddLast(column);
                }
                else
                {
                    addedCollumns.Add(column);
                }

                var select = selects.FirstOrDefault(e => e.Columns.Contains(column));

                yield return new ColumnSerialization(type, i, column, select?.PropertyHierarchy, dbSettings);
            }
        }
    }
}
