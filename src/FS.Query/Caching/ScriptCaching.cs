using Microsoft.Extensions.Caching.Memory;
using System;

namespace FS.Query.Caching
{
    public class ScriptCaching
    {
        private readonly MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions { });
        private readonly bool enableCaching;
        private readonly TimeSpan maxInactiveTime;

        public ScriptCaching(bool enableCaching, TimeSpan maxInactiveTime)
        {
            this.enableCaching = enableCaching;
            this.maxInactiveTime = maxInactiveTime;
        }

        public Script GetOrCreate(CacheKey cacheKey, Func<ICacheEntry?, Script> factory)
        {
            if (!enableCaching)
                return factory(null);

            return memoryCache.GetOrCreate(
                cacheKey,
                entry =>
                {
                    entry.SlidingExpiration = maxInactiveTime;
                    return factory(entry);
                });
        }
    }
}
