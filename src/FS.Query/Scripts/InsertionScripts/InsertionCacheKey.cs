using FS.Query.Scripts.Columns;
using FS.Query.Scripts.InsertionScripts.Entries;
using FS.Query.Scripts.SelectionScripts.Sources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.Query.Scripts.InsertionScripts
{
    public struct InsertionCacheKey
    {
        private readonly int hashCode;

        public Table Table { get; }
        public IEnumerable<IColumn> Columns { get; }
        public Entry Entry { get; }

        public InsertionCacheKey(
            Table table,
            IEnumerable<IColumn> columns,
            Entry entry)
        {
            hashCode = CreateHashCode(table, columns, entry);
            Table = table;
            Columns = columns;
            Entry = entry;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not InsertionCacheKey cacheKey || cacheKey.Table != Table || cacheKey.Entry != Entry) return false;

            return Columns.SequenceEqual(cacheKey.Columns);
        }

        public override int GetHashCode() => hashCode;

        private static int CreateHashCode(Table table, IEnumerable<IColumn> columns, Entry entry)
        {
            int hash = HashCode.Combine(table, entry);

            foreach (var column in columns) hash = HashCode.Combine(hash, column);

            return hash;
        }

        public static bool operator ==(InsertionCacheKey left, InsertionCacheKey right) => left.Equals(right);
        public static bool operator !=(InsertionCacheKey left, InsertionCacheKey right) => !(left == right);
    }
}
