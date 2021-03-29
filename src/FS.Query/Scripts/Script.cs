using FS.Query.Scripts;
using FS.Query.Scripts.Combinations;
using FS.Query.Scripts.Filters;
using FS.Query.Scripts.Operators;
using FS.Query.Scripts.Parameters;
using FS.Query.Scripts.Sources;
using FS.Query.Settings;
using FS.Query.Settings.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Query
{
    public class Script
    {
        private LinkedList<IScriptColumn>? selectedColumns;
        private LinkedList<Combination>? combinations;
        private LinkedList<ComparationBlock>? filters;
        private ScriptParameters? scriptParameters;

        public Script(Source from) => From = from;

        public Source From { get; }
        public LinkedList<IScriptColumn> SelectedColumns { get => selectedColumns ??= new(); }
        public LinkedList<Combination> Combinations { get => combinations ??= new(); }
        public LinkedList<ComparationBlock> Filters { get => filters ??= new(); }
        public ScriptParameters ScriptParameters { get => scriptParameters ??= new(); }

        public BuildedScript Build(DbSettings dbSettings)
        {
            var select = BuildSelect(dbSettings);
            var from = From.Build(dbSettings);
            var joins = BuildJoins(dbSettings);
            var where = BuildWhere(dbSettings);

            var stringBuilder = new StringBuilder()
                .Append(select)
                .Append(" FROM ")
                .Append(from)
                .Append(joins)
                .Append(where);

            return new BuildedScript(stringBuilder.ToString(), ScriptParameters);
        }

        private object BuildSelect(DbSettings dbSettings)
        {
            if (SelectedColumns.Count == 0)
                return "SELECT * ";

            var stringBuilder = new StringBuilder("SELECT ");

            int index = 0;
            foreach (var property in SelectedColumns)
            {
                stringBuilder.Append(property.Build(dbSettings));

                if (++index < SelectedColumns.Count)
                    stringBuilder.Append(", ");
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

        public CacheKey GetKey() => new(From, Combinations.ToArray(), SelectedColumns.ToArray(), Filters.ToArray());
    }
}
