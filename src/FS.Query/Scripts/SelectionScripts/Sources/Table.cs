using FS.Query.Settings;
using System;
using System.Collections.Generic;

namespace FS.Query.Scripts.SelectionScripts.Sources
{
    public class Table
    {
        public Table(Type type)
        {
            Type = type;
        }

        public Type Type { get; }

        public object Build(DbSettings dbSettings)
        {
            var map = dbSettings.MapCaching.GetOrCreate(Type, dbSettings);
            return map.TableFullName;
        }

        public override bool Equals(object? obj) =>
            obj is Table table && EqualityComparer<Type>.Default.Equals(Type, table.Type);

        public override int GetHashCode() => HashCode.Combine(Type);
    }
}
