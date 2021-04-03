using FS.Query.Settings;
using System;
using System.Collections.Generic;

namespace FS.Query.Scripts.Sources
{
    public class Table : Source, ITypedSource
    {
        public Table(Type type, string alias) : base(alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentException($"'{nameof(alias)}' cannot be null or whitespace.", nameof(alias));

            Type = type;
        }

        public Type Type { get; }

        public override object Build(DbSettings dbSettings)
        {
            var map = dbSettings.MapCaching.GetOrCreate(Type, dbSettings);
            return $"{map.TableFullName} {TreatedAlias}";
        }

        public override bool Equals(object? obj) =>
            obj is Table table && EqualityComparer<Type>.Default.Equals(Type, table.Type);

        public override int GetHashCode() => HashCode.Combine(Type);
    }
}
