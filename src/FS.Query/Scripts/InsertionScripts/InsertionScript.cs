using FS.Query.Scripts.Columns;
using FS.Query.Scripts.InsertionScripts.Entries;
using FS.Query.Scripts.SelectionScripts.Sources;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Query.Scripts.InsertionScripts
{
    public class InsertionScript
    {
        private LinkedList<IColumn>? columns;

        public InsertionScript(Table table)
        {
            Table = table;
        }

        public Table Table { get; }
        public LinkedList<IColumn> Columns => columns ??= new();
        public Entry? Entry { get; set; }

        public BuildedInsertionScript Build(DbSettings dbSettings)
        {
            if (Table is null)
                throw new ArgumentException("Table can't be null");

            var columnsArray = Columns.ToArray();
            var buildedColumns = BuildColumns(dbSettings, columnsArray);
            var buildedTable = Table.Build(dbSettings);

            if (buildedTable is null)
                throw new Exception("The builded table can't be null.");

            var stringBuilder = new StringBuilder("INSERT INTO ")
                .Append(buildedTable)
                .Append(' ')
                .Append(buildedColumns)
                .Append(" VALUES @entry");

            var script = stringBuilder.ToString();

            return new(script, columnsArray);
        }

        private StringBuilder BuildColumns(DbSettings dbSettings, IColumn[] columns)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append('(');
            var lastIndex = columns.Length - 1;
            for (int i = 0; i < columns.Length; i++)
            {
                stringBuilder.Append(columns[i].Build(dbSettings));

                if (i != lastIndex)
                    stringBuilder.Append(", ");
            }
            stringBuilder.Append(')');

            return stringBuilder;
        }

        public InsertionCacheKey GetKey()
        {
            if (Entry is null)
                throw new ArgumentException("There is no entry in the script.");

            return new InsertionCacheKey(Table, Columns, Entry);
        }
    }
}
