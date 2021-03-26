using System;
using System.Linq;

namespace FS.Query.Caching
{
    public struct CacheKey
    {
        private static readonly Join[] EmptyJoinArray = Array.Empty<Join>();
        private static readonly string[] EmptyStringArray = Array.Empty<string>();
        private readonly object fromId;
        private readonly Join[] joins;
        private readonly string[] properties;
        private readonly int hashCode;

        public CacheKey(object fromId, Join[]? joins = null, string[]? properties = null)
        {
            this.fromId = fromId;
            this.joins = joins ?? EmptyJoinArray;
            this.properties = properties ?? EmptyStringArray;
            hashCode = CreateHashCode(fromId, this.joins, this.properties);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not CacheKey cacheKey || cacheKey.fromId != fromId) return false;

            return 
                joins.SequenceEqual(cacheKey.joins) &&
                properties.SequenceEqual(cacheKey.properties);
        }

        public override int GetHashCode() => hashCode;

        private static int CreateHashCode(object fromId, Join[] joins, string[] properties)
        {
            int hash = fromId.GetHashCode();

            foreach (var join in joins) hash = HashCode.Combine(hash, join);
            foreach (var property in properties) hash = HashCode.Combine(hash, property);

            return hash;
        }

        public static bool operator ==(CacheKey left, CacheKey right) => left.Equals(right);
        public static bool operator !=(CacheKey left, CacheKey right) => !left.Equals(right);
    }
}
