using System;
using System.Collections.Generic;
using System.Linq;
using FS.Query.Scripts.Columns;

namespace FS.Query.Scripts.SelectionScripts.Selects
{
    public class Select
    {
        private static readonly IEnumerable<IAliasColumn> emptyColumns = Enumerable.Empty<IAliasColumn>();
        private IEnumerable<IAliasColumn>? columns;

        public Select(string tableAlias, bool selectEverything)
        {
            TableAlias = tableAlias;
            SelectEverything = selectEverything;
        }

        public Select(string tableAlias, IEnumerable<IAliasColumn> columns)
        {
            TableAlias = tableAlias;
            this.columns = columns;
        }

        public virtual IEnumerable<IAliasColumn> Columns => columns ??= emptyColumns;
        public virtual string TableAlias { get; }
        public virtual bool SelectEverything { get; }
        public virtual string[]? PropertyHierarchy { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Select selectColumn)
                return false;

            if (PropertyHierarchy is null ^ selectColumn.PropertyHierarchy is null)
                return false;

            if (PropertyHierarchy is null)
                return true;

            return
                Columns.SequenceEqual(selectColumn.Columns) &&
                PropertyHierarchy.SequenceEqual(selectColumn.PropertyHierarchy!);
        }

        public override int GetHashCode()
        {
            var hash = 13;

            foreach (var column in Columns)
                hash = HashCode.Combine(hash, column);

            if (PropertyHierarchy is null)
                return hash;

            foreach (var property in PropertyHierarchy)
                hash = HashCode.Combine(hash, property);

            return hash;
        }
    }
}
