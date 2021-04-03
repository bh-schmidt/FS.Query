using FS.Query.Builders.Filters;
using FS.Query.Builders.Orders;
using FS.Query.Builders.Selects;
using FS.Query.Scripts;
using FS.Query.Scripts.Combinations.Joins;
using FS.Query.Scripts.Runners;
using FS.Query.Scripts.Sources;
using System;

namespace FS.Query.Builders
{
    public class SelectionBuilder
    {
        private readonly Source source;
        private readonly SelectionScript script;
        private readonly DbManager dbManager;

        public SelectionBuilder(Source source, SelectionScript script, DbManager dbManager)
        {
            this.source = source;
            this.script = script;
            this.dbManager = dbManager;
        }

        public SelectionByTableBuilder<TJoinType> Join<TJoinType>(string alias, Action<ComparationBlockBuilder> builder)
        {
            var comparationBlockBuilder = new ComparationBlockBuilder(script);
            builder(comparationBlockBuilder);

            var joinTable = new Table(typeof(TJoinType), alias);
            var join = new ComparationBlockJoin(source, joinTable, comparationBlockBuilder.ComparationBlock);
            script.Combinations.AddLast(join);
            return new SelectionByTableBuilder<TJoinType>(joinTable, script, dbManager);
        }

        public SelectionBuilder Limit(long limit)
        {
            script.Limit = limit;
            return this;
        }

        public SelectionBuilder Order(Action<OrderBuilder> builder)
        {
            var orderBuilder = new OrderBuilder(script);
            builder(orderBuilder);
            return this;
        }

        public ScriptRunner<T> Select<T>()
        {
            var buildedScript = BuildScript();
            return new ScriptRunner<T>(buildedScript, dbManager);
        }

        public ScriptRunner<T> Select<T>(Action<SelectBuilder<T>> buildSelect)
        {
            var selectBuilder = new SelectBuilder<T>(script);
            buildSelect(selectBuilder);
            var buildedScript = BuildScript();
            return new ScriptRunner<T>(buildedScript, dbManager);
        }

        public SelectionBuilder Where(Action<ComparationBlockBuilder> builder)
        {
            var comparationBlockBuilder = new ComparationBlockBuilder(script);
            builder(comparationBlockBuilder);
            script.Filters.AddLast(comparationBlockBuilder.ComparationBlock);
            return this;
        }

        protected BuildedScript BuildScript() =>
            dbManager
                .DbSettings
                .ScriptCache
                .GetOrCreate(script, dbManager.DbSettings);
    }
}
