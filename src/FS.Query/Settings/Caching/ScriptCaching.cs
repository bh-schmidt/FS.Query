using FS.Query.Scripts;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FS.Query.Settings.Caching
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

        public BuildedScript GetOrCreate(SelectionScript script, DbSettings dbSettings)
        {
            if (!enableCaching)
                return script.Build(dbSettings);

            var cacheKey = script.GetKey();

            return memoryCache.GetOrCreate(
                cacheKey,
                entry =>
                {
                    entry.SlidingExpiration = maxInactiveTime;
                    return script.Build(dbSettings);
                });
        }
    }
}
