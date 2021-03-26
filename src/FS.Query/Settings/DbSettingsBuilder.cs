using FS.Query.Caching;
using FS.Query.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FS.Query.Settings
{
    public class DbSettingsBuilder
    {
        private List<ObjectMap> objectMaps = new List<ObjectMap>();
        private readonly TimeSpan DefaultInactiveTime = TimeSpan.FromMinutes(10);
        private readonly DbSettings Settings = new DbSettings();

        public DbSettingsBuilder WithMapCache(TimeSpan maxInactiveTime)
        {
            Settings.MapCaching = new MapCaching(maxInactiveTime);
            return this;
        }

        public DbSettingsBuilder WithConnection(Func<IServiceProvider, IDbConnection> createConnection)
        {
            Settings.Connection = new Connection(createConnection);
            return this;
        }

        public DbSettingsBuilder WithScriptCache(bool enable = true, TimeSpan? maxInactiveTime = null)
        {
            maxInactiveTime ??= DefaultInactiveTime;
            Settings.ScriptCache = new ScriptCaching(enable, maxInactiveTime.Value);
            return this;
        }

        public DbSettingsBuilder Map<TMap>()
            where TMap : class, IMap, new()
        {
            var map = new TMap();
            if (objectMaps.Any(e => e.Type == typeof(TMap)))
                return this;

            objectMaps.Add(map.ObjectMap);

            return this;

        }

        public DbSettings Build()
        {
            if (Settings.MapCaching is null)
                Settings.MapCaching = new MapCaching(DefaultInactiveTime);

            if (Settings.ScriptCache is null)
                Settings.ScriptCache = new ScriptCaching(true, DefaultInactiveTime);

            objectMaps.ForEach(e => Settings.MapCaching.AddPermanently(e));

            return Settings;
        }

        public static DbSettingsBuilder Create()  => new DbSettingsBuilder();
    }
}
