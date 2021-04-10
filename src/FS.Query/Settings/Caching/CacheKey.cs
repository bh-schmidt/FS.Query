using FS.Query.Scripts.SelectionScripts.Combinations;
using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Scripts.SelectionScripts.Selects;
using FS.Query.Scripts.SelectionScripts.Sources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.Query.Settings.Caching
{
    public struct CacheKey
    {
        private readonly ISource source;
        private readonly IEnumerable<Combination> combinations;
        private readonly IEnumerable<Select> selectColumns;
        private readonly IEnumerable<ComparationBlock> comparationBlocks;
        private readonly long limit;
        private readonly int hashCode;

        public CacheKey(
            ISource source,
            IEnumerable<Combination> combinations,
            IEnumerable<Select> selectColumns,
            IEnumerable<ComparationBlock> comparationBlocks,
            long limit)
        {
            this.source = source;
            this.combinations = combinations;
            this.selectColumns = selectColumns;
            this.comparationBlocks = comparationBlocks;
            this.limit = limit;
            hashCode = CreateHashCode(source, this.combinations, this.selectColumns, comparationBlocks, limit);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not CacheKey cacheKey || cacheKey.source != source || cacheKey.limit != limit) return false;

            return
                combinations.SequenceEqual(cacheKey.combinations) &&
                selectColumns.SequenceEqual(cacheKey.selectColumns) &&
                comparationBlocks.SequenceEqual(cacheKey.comparationBlocks);
        }

        public override int GetHashCode() => hashCode;

        private static int CreateHashCode(object fromId, IEnumerable<Combination> combinations, IEnumerable<Select> selectColumns, IEnumerable<ComparationBlock> comparationBlocks, long limit)
        {
            int hash = HashCode.Combine(fromId, limit);

            foreach (var combination in combinations) hash = HashCode.Combine(hash, combination);
            foreach (var column in selectColumns) hash = HashCode.Combine(hash, column);
            foreach (var comparationBlock in comparationBlocks) hash = HashCode.Combine(hash, comparationBlock);

            return hash;
        }

        public static bool operator ==(CacheKey left, CacheKey right) => left.Equals(right);
        public static bool operator !=(CacheKey left, CacheKey right) => !(left == right);
    }
}
