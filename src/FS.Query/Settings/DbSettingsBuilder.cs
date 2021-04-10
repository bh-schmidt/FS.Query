using FS.Query.Settings.Builders;
using FS.Query.Settings.Caching;
using FS.Query.Settings.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FS.Query.Settings
{
    public class DbSettingsBuilder
    {
        private readonly TimeSpan DefaultInactiveTime = TimeSpan.FromHours(1);
        private LinkedList<ObjectMap>? objectMaps;
        private DbSettings? settings = new();

        public LinkedList<ObjectMap> ObjectMaps { get => objectMaps ??= new(); }
        public DbSettings Settings { get => settings ??= new(); }

        public virtual DbSettingsBuilder WithMapCache(TimeSpan maxInactiveTime)
        {
            Settings.MapCaching = new MapCaching(maxInactiveTime);
            return this;
        }

        public virtual DbSettingsBuilder WithConnection(Func<IServiceProvider, IDbConnection> createConnection)
        {
            Settings.Connection = new Connection(createConnection);
            return this;
        }

        public virtual DbSettingsBuilder WithScriptCache(bool enable = true, TimeSpan? maxInactiveTime = null)
        {
            maxInactiveTime ??= DefaultInactiveTime;
            Settings.ScriptCache = new ScriptCaching(enable, maxInactiveTime.Value);
            return this;
        }

        public virtual DbSettingsBuilder Map<TMap>()
            where TMap : class, ITableMap, new()
        {
            var map = new TMap();
            if (ObjectMaps.Any(e => e.Type == typeof(TMap)))
                return this;

            ObjectMaps.AddLast(map.ObjectMap);

            return this;
        }

        public virtual DbSettings Build()
        {
            if (Settings.MapCaching is null)
                Settings.MapCaching = new MapCaching(DefaultInactiveTime);

            if (Settings.ScriptCache is null)
                Settings.ScriptCache = new ScriptCaching(true, DefaultInactiveTime);

            if (Settings.TypeMapping is null)
                Settings.TypeMapping = new TypeMapping();

            if (Settings.ScriptBuilder is null)
                Settings.ScriptBuilder = new ScriptBuilder();

            foreach (var objectMap in ObjectMaps)
                Settings.MapCaching.AddPermanently(objectMap, Settings);

            return Settings;
        }

        public static DbSettingsBuilder Create() => new();
    }
}
