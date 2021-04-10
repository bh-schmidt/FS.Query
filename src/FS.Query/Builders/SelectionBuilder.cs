using FS.Query.Builders.Filters;
using FS.Query.Builders.Orders;
using FS.Query.Builders.Selections;
using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Combinations.Joins;
using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Scripts.SelectionScripts.Runners;
using FS.Query.Scripts.SelectionScripts.Selects;
using FS.Query.Scripts.SelectionScripts.Sources;
using FS.Query.Settings;
using System;
using System.Collections.Generic;

namespace FS.Query.Builders
{
    public class SelectionBuilder
    {
        private readonly Source source;
        private readonly SelectionScript selectionScript;
        private readonly DbManager dbManager;

        public SelectionBuilder(Source source, SelectionScript selectionScript, DbManager dbManager)
        {
            this.source = source;
            this.selectionScript = selectionScript;
            this.dbManager = dbManager;
        }

        public SelectionByTableBuilder<TJoinType> Join<TJoinType>(string alias, Action<ComparationBlockBuilder> builder)
        {
            var comparationBlockBuilder = new ComparationBlockBuilder(selectionScript);
            builder(comparationBlockBuilder);

            var joinTable = new Table(typeof(TJoinType), alias);
            var join = new ComparationBlockJoin(source, joinTable, comparationBlockBuilder.ComparationBlock);
            selectionScript.Combinations.AddLast(join);
            return new SelectionByTableBuilder<TJoinType>(joinTable, selectionScript, dbManager);
        }

        public SelectionBuilder Limit(long limit)
        {
            selectionScript.Limit = limit;
            return this;
        }

        public SelectionBuilder Order(Action<OrderBuilder> builder)
        {
            var orderBuilder = new OrderBuilder(selectionScript);
            builder(orderBuilder);
            return this;
        }

        public ScriptRunner<T> Select<T>()
        {
            SetDefaultSelection(dbManager.DbSettings, selectionScript);

            var buildedScript = BuildScript();
            return new ScriptRunner<T>(buildedScript, dbManager);
        }

        public ScriptRunner<T> Select<T>(Action<SelectionScriptBuilder<T>> buildSelect)
        {
            var selectBuilder = new SelectionScriptBuilder<T>(selectionScript);
            buildSelect(selectBuilder);

            SetDefaultSelection(dbManager.DbSettings, selectionScript);

            var buildedScript = BuildScript();
            return new ScriptRunner<T>(buildedScript, dbManager);
        }

        public SelectionBuilder Where(Action<ComparationBlockBuilder> builder)
        {
            var comparationBlockBuilder = new ComparationBlockBuilder(selectionScript);
            builder(comparationBlockBuilder);
            selectionScript.Filters.AddLast(comparationBlockBuilder.ComparationBlock);
            return this;
        }

        protected BuildedScript BuildScript() =>
            dbManager
                .DbSettings
                .ScriptCache
                .GetOrCreate(selectionScript, dbManager.DbSettings);

        private static void SetDefaultSelection(DbSettings dbSettings, SelectionScript selectionScript)
        {
            if (selectionScript.Selects.Count == 0 && selectionScript.From is Table table)
                SelectEverything(dbSettings, selectionScript, table);

            if (selectionScript.Selects.Count == 0)
            {
                var select = new Select(selectionScript.From.Alias, true);
                selectionScript.Selects.AddLast(select);
            }
        }

        private static void SelectEverything(DbSettings dbSettings, SelectionScript selectionScript, Table table)
        {
            var map = dbSettings.MapCaching.GetOrCreate(table.Type, dbSettings);
            var columns = new List<IScriptColumn>(map.PropertyMaps.Count);
            foreach (var property in map.PropertyMaps)
            {
                var scriptColumn = new TableProperty(table.Alias, table.Type, property.PropertyName);
                columns.Add(scriptColumn);
            }
            var selectColumn = new Select(table.Alias, columns);
            selectionScript.Selects.AddLast(selectColumn);
        }
    }
}
