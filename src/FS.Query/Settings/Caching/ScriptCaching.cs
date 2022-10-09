using FS.Query.Scripts.InsertionScripts;
using FS.Query.Scripts.SelectionScripts;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FS.Query.Settings.Caching
{
    public class ScriptCaching
    {
        private readonly MemoryCache memoryCache = new(new MemoryCacheOptions { });
        private readonly bool enableCaching;
        private readonly TimeSpan maxInactiveTime;

        public ScriptCaching(bool enableCaching, TimeSpan maxInactiveTime)
        {
            this.enableCaching = enableCaching;
            this.maxInactiveTime = maxInactiveTime;
        }

        public virtual BuildedInsertionScript GetOrCreate(InsertionScript insertionScript, DbSettings dbSettings)
        {
            if (!enableCaching)
                return insertionScript.Build(dbSettings);

            var cacheKey = insertionScript.GetKey();

            return memoryCache.GetOrCreate(
                cacheKey,
                entry =>
                {
                    entry.SlidingExpiration = maxInactiveTime;
                    return insertionScript.Build(dbSettings);
                });
        }

        public virtual BuildedSelectionScript GetOrCreate(SelectionScript selectionScript, DbSettings dbSettings)
        {
            if (!enableCaching)
                return selectionScript.Build(dbSettings);

            var cacheKey = selectionScript.GetKey();

            return memoryCache.GetOrCreate(
                cacheKey,
                entry =>
                {
                    entry.SlidingExpiration = maxInactiveTime;
                    return selectionScript.Build(dbSettings);
                });
        }
    }
}
