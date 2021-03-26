using FS.Query.Factory;
using FS.Query.Factory.Filters;
using FS.Query.Mapping;
using FS.Query.Settings;
using System.Collections.Generic;
using System.Text;

namespace FS.Query
{
    public class Script
    {
        private readonly List<PropertyMap> SelectedProperties = new List<PropertyMap>();

        public Script(Source from)
        {
            From = from;
        }

        public Source From { get; }
        public LinkedList<Join> Joins { get; } = new LinkedList<Join>();
        public LinkedList<ComparationBlock> Filters = new LinkedList<ComparationBlock>();

        public PropertyMap GetProperty(int index) => SelectedProperties[index];

        public string Build(DbSettings dbSettings)
        {
            var properties = new LinkedList<string>();
            var fromBuilder = new StringBuilder("FROM ")
                .Append(From.Build(dbSettings));

            AddProperties(From, properties, dbSettings);

            foreach (var join in Joins)
            {
                fromBuilder
                    .Append(' ')
                    .Append(join.Build(dbSettings));

                AddProperties(join.SecondSource, properties, dbSettings);
            }

            StringBuilder selectBuilder;
            if (properties.Count == 0)
                selectBuilder = new StringBuilder("SELECT * ");
            else
            {
                var join = string.Join(", ", properties);
                selectBuilder = new StringBuilder($"SELECT {join} ");
            }

            selectBuilder.Append(fromBuilder);

            return selectBuilder.ToString();
        }

        private void AddProperties(Source source, LinkedList<string> properties, DbSettings dbSettings)
        {
            ObjectMap? objectMap = null;

            if (source is ITypedSource typedSource)
                objectMap = dbSettings.MapCaching.GetOrCreate(typedSource.Type);

            foreach (var property in source.PropertiesToSelect)
            {
                string? columnName = null;

                if (objectMap is not null)
                {
                    var propMap = objectMap.PropertiesToColumns.GetValueOrDefault(property);
                    columnName = propMap?.ColumnName;
                }

                columnName ??= $"[{columnName}]";

                properties.AddLast($"{source.TreatedAlias}.{columnName}");

            }
        }
    }
}
