using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.Query.Scripts.Selects
{
    public class Select
    {
        private static readonly IEnumerable<IScriptColumn> emptyColumns = Enumerable.Empty<IScriptColumn>();
        private string[]? propertyHierarchy;
        private string? buildedHierarchy;
        private IEnumerable<IScriptColumn>? columns;

        public Select(string tableAlias, bool selectEverything)
        {
            TableAlias = tableAlias;
            SelectEverything = selectEverything;
        }

        public Select(string tableAlias, IEnumerable<IScriptColumn> columns)
        {
            TableAlias = tableAlias;
            this.columns = columns;
        }

        public IEnumerable<IScriptColumn> Columns => columns ??= emptyColumns;
        public string TableAlias { get; }
        public bool SelectEverything { get; }
        public string[]? PropertyHierarchy
        {
            get => propertyHierarchy;
            set
            {
                buildedHierarchy = null;
                propertyHierarchy = value;
            }
        }

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
