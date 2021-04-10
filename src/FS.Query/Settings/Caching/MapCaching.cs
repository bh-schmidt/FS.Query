using FS.Query.Settings.Mapping;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FS.Query.Settings.Caching
{
    public class MapCaching
    {
        private readonly IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
        private readonly TimeSpan objectMapMaxInactiveTime;

        public MapCaching(TimeSpan objectMapMaxInactiveTime)
        {
            this.objectMapMaxInactiveTime = objectMapMaxInactiveTime;
        }

        public void AddPermanently(ObjectMap objectMap, DbSettings dbSettings)
        {
            objectMap.Build(dbSettings);
            memoryCache.Set(objectMap.Type, objectMap, new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });
        }

        public virtual ObjectMap GetOrCreate(Type type, DbSettings dbSettings) =>
            memoryCache.GetOrCreate(
                type,
                entry =>
                {
                    entry.SlidingExpiration = objectMapMaxInactiveTime;
                    return CreateObjectMap(type, dbSettings);
                });

        private static ObjectMap CreateObjectMap(Type type, DbSettings dbSettings)
        {
            var map = new ObjectMap(type);

            foreach (var property in type.GetProperties())
            {
                var propertyMap = new PropertyMap(property.Name, property.PropertyType);
                map.PropertyMaps.Add(propertyMap);
            }

            map.Build(dbSettings);
            return map;
        }
    }
}
