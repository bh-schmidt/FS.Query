using FS.Query.Scripts;
using FS.Query.Scripts.Combinations;
using FS.Query.Scripts.Filters;
using FS.Query.Scripts.Joins;
using FS.Query.Scripts.Sources;
using System;
using System.Linq;

namespace FS.Query.Settings.Caching
{
    public struct CacheKey
    {
        private readonly ISource source;
        private readonly Combination[] combinations;
        private readonly IScriptColumn[] columns;
        private readonly ComparationBlock[] comparationBlocks;
        private readonly int hashCode;

        public CacheKey(
            ISource source,
            Combination[] combination,
            IScriptColumn[] properties,
            ComparationBlock[] comparationBlocks)
        {
            this.source = source;
            this.combinations = combination;
            this.columns = properties;
            this.comparationBlocks = comparationBlocks;
            hashCode = CreateHashCode(source, combinations, columns, comparationBlocks);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not CacheKey cacheKey || cacheKey.source != source) return false;

            return
                combinations.SequenceEqual(cacheKey.combinations) &&
                columns.SequenceEqual(cacheKey.columns) &&
                comparationBlocks.SequenceEqual(cacheKey.comparationBlocks);
        }

        public override int GetHashCode() => hashCode;

        private static int CreateHashCode(object fromId, Combination[] combinations, IScriptColumn[] properties, ComparationBlock[] comparationBlocks)
        {
            int hash = fromId.GetHashCode();

            foreach (var combination in combinations) hash = HashCode.Combine(hash, combination);
            foreach (var property in properties) hash = HashCode.Combine(hash, property);
            foreach (var comparationBlock in comparationBlocks) hash = HashCode.Combine(hash, comparationBlock);

            return hash;
        }
    }
}
