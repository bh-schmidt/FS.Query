using FS.Query.Builders.Filters;
using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.Filters;
using FS.Query.Scripts.Joins;
using FS.Query.Scripts.Runners;
using FS.Query.Scripts.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FS.Query.Builders
{
    public class TableSelectBuilder<TTable>
    {
        private readonly Script script;
        private readonly Table table;
        private readonly DbManager databaseManager;

        public TableSelectBuilder(Table table, Script script, DbManager databaseManager)
        {
            this.script = script;
            this.table = table;
            this.databaseManager = databaseManager;
        }

        public TableSelectBuilder<TTable> Select(params Expression<Func<TTable, object?>>[] selectProperties)
        {
            if (selectProperties is null)
                return this;

            foreach (var expression in selectProperties)
            {
                var propertyInfo = expression.GetPropertyInfo();
                var tableProperty = new TableProperty(table.Alias, table.Type, propertyInfo.Name);
                script.SelectedColumns.AddLast(tableProperty);
            }

            return this;
        }

        public IEnumerable<TType> Execute<TType>()
            where TType : new() =>
            ScriptRunner.Run<TType>(script, databaseManager);

        public TableSelectBuilder<TJoinType> Join<TJoinType>(string alias, Expression<Func<TTable, TJoinType, bool>> where)
        {
            var joinTable = new Table(typeof(TJoinType), alias);
            var join = new ExpressionJoin(table, joinTable, where.Parameters.ToArray(), where.Body);
            script.Combinations.AddLast(join);
            return new TableSelectBuilder<TJoinType>(joinTable, script, databaseManager);
        }

        public TableSelectBuilder<TTable> Where(Action<ComparationBlockBuilder> builder)
        {
            var comparationBlockBuilder = new ComparationBlockBuilder(script);
            builder(comparationBlockBuilder);
            script.Filters.AddLast(comparationBlockBuilder.ComparationBlock);
            return this;
        }
    }
}
