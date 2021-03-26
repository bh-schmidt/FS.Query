using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FS.Query.Factory.Sources
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
        public LinkedList<string> PropertyNames { get; } = new LinkedList<string>();

        public override StringBuilder Build(DbSettings dbSettings)
        {
            var map = dbSettings.MapCaching.GetOrCreate(Type);
            return new StringBuilder($"{map.TableFullName} {TreatedAlias}");
        }

        public override object GetSourceId() => Type;
    }
}
