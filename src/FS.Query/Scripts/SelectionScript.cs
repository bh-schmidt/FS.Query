using FS.Query.Scripts;
using FS.Query.Scripts.Combinations;
using FS.Query.Scripts.Filters;
using FS.Query.Scripts.Operators;
using FS.Query.Scripts.Orders;
using FS.Query.Scripts.Parameters;
using FS.Query.Scripts.Selects;
using FS.Query.Scripts.Sources;
using FS.Query.Settings;
using FS.Query.Settings.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Query
{
    public class SelectionScript
    {
        private LinkedList<Select>? selects;
        private LinkedList<Combination>? combinations;
        private LinkedList<ComparationBlock>? filters;
        private ScriptParameters? scriptParameters;
        private LinkedList<ColumnOrder>? orders;

        public SelectionScript(Source from) => From = from;

        public Source From { get; }
        public LinkedList<Select> Selects => selects ??= new();
        public LinkedList<Combination> Combinations => combinations ??= new();
        public LinkedList<ComparationBlock> Filters => filters ??= new();
        public LinkedList<ColumnOrder> Orders => orders ??= new();
        public ScriptParameters ScriptParameters => scriptParameters ??= new();
        public long? Limit { get; set; }

        public BuildedScript Build(DbSettings dbSettings)
        {
            var limit = BuildLimit();
            var select = BuildSelect(dbSettings);
            var from = From.Build(dbSettings);
            var joins = BuildJoins(dbSettings);
            var where = BuildWhere(dbSettings);
            var order = BuildOrder(dbSettings);

            var stringBuilder = new StringBuilder("SELECT ")
                .Append(limit)
                .Append(select)
                .Append(" FROM ")
                .Append(from)
                .Append(joins)
                .Append(where)
                .Append(order);

            return new BuildedScript(stringBuilder.ToString(), Selects.ToArray(), ScriptParameters);
        }

        private object BuildLimit()
        {
            if (Limit is null)
                return "";

            if (Limit < 1)
                throw new Exception("Limit can't be lower than 1");

            return $"TOP {Limit} ";
        }

        private object BuildSelect(DbSettings dbSettings)
        {
            if (Selects.Count == 0)
            {
                if (From is Table table)
                {
                    var map = dbSettings.MapCaching.GetOrCreate(table.Type, dbSettings);
                    var columns = new List<IScriptColumn>(map.PropertyMaps.Count);
                    foreach (var property in map.PropertyMaps)
                    {
                        var scriptColumn = new TableProperty(table.Alias, table.Type, property.PropertyName);
                        columns.Add(scriptColumn);
                    }
                    var selectColumn = new Select(table.Alias, columns);
                    Selects.AddLast(selectColumn);
                }
                else
                {
                    return '*';
                }
            }

            var stringBuilder = new StringBuilder();

            var addComma = false;
            foreach (var select in Selects)
                foreach (var column in select.Columns)
                {
                    if (addComma)
                        stringBuilder.Append(", ");
                    else
                        addComma = true;
                    stringBuilder.Append(column.Build(dbSettings));
                }

            return stringBuilder;
        }

        private object BuildJoins(DbSettings dbSettings)
        {
            var builder = new StringBuilder();

            foreach (var join in Combinations)
                builder
                    .Append(' ')
                    .Append(join.Build(dbSettings));

            return builder;
        }

        private object BuildWhere(DbSettings dbSettings)
        {
            if (Filters.Count == 0)
                return string.Empty;

            var builder = new StringBuilder();
            builder.Append(" WHERE ");

            var filters = Filters.ToArray();

            int index = 0;
            foreach (var filter in filters)
            {
                var blockBuilder = filter.Build(dbSettings);
                builder.Append(blockBuilder);

                if (++index < filters.Length)
                    builder.Append($" {Operator.And.Operator} ");
            }

            return builder;
        }

        private object BuildOrder(DbSettings dbSettings)
        {
            if (Orders.Count == 0)
                return string.Empty;

            var builder = new StringBuilder();
            builder.Append(" ORDER BY ");

            var addComma = false;
            foreach (var order in Orders)
            {
                if (addComma)
                    builder.Append($", ");
                else
                    addComma = true;

                builder.Append(order.ScriptColumn.Build(dbSettings));

                if (order.Descending)
                    builder.Append(" DESC");
            }

            return builder;
        }

        public CacheKey GetKey() => new(From, Combinations.ToArray(), Selects.ToArray(), Filters.ToArray(), Limit ?? 0);
    }
}
