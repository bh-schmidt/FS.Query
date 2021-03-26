using FS.Query.Mapping;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FS.Query.Caching
{
    public class MapCaching
    {
        private readonly IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
        private readonly TimeSpan objectMapMaxInactiveTime;

        public MapCaching(TimeSpan objectMapMaxInactiveTime)
        {
            this.objectMapMaxInactiveTime = objectMapMaxInactiveTime;
        }

        public void AddPermanently(ObjectMap objectMap)
        {
            objectMap.Build();
            memoryCache.Set(objectMap.Type, objectMap, new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });
        }

        public ObjectMap GetOrCreate(Type type) =>
            memoryCache.GetOrCreate(
                type,
                entry =>
                {
                    entry.SlidingExpiration = objectMapMaxInactiveTime;
                    return CreateObjectMap(type);
                });

        private static ObjectMap CreateObjectMap(Type type)
        {
            var map = new ObjectMap(type);

            foreach (var property in type.GetProperties())
            {
                var propertyMap = new PropertyMap(property.PropertyType, property.Name);
                map.PropertyMaps.Add(propertyMap);
            }

            map.Build();
            return map;
        }
    }
}
