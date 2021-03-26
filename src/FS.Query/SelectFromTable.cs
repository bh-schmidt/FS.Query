using FS.Query.Builders.Filters;
using FS.Query.Extensions;
using FS.Query.Factory;
using FS.Query.Factory.Sources;
using FS.Query.Runners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FS.Query
{
    public class SelectFromTable<TTable>
    {
        private readonly Script script;
        private readonly Table table;
        private readonly DatabaseManager databaseManager;

        public SelectFromTable(Table table, Script script, DatabaseManager databaseManager)
        {
            this.script = script;
            this.table = table;
            this.databaseManager = databaseManager;
        }

        public SelectFromTable<TTable> Select<TProperty>(Expression<Func<TTable, TProperty>> selectProperty)
        {
            var propertyInfo = selectProperty.GetPropertyInfo();
            table.PropertiesToSelect.AddLast(propertyInfo.Name);
            return this;
        }

        public IEnumerable<TType> Execute<TType>()
            where TType : new() =>
            ScriptRunner.Run<TType>(script, databaseManager);

        public SelectFromTable<TJoinType> Join<TJoinType>(string alias, Expression<Func<TTable, TJoinType, bool>> where)
        {
            var joinTable = new Table(typeof(TJoinType), alias);
            var join = new Join(table, joinTable, where.Parameters.ToArray(), where.Body);
            script.Joins.AddLast(join);
            return new SelectFromTable<TJoinType>(joinTable, script, databaseManager);
        }

        public SelectFromTable<TTable> Where(Action<ComparationBlockBuilder> builder)
        {
            var comparationBlockBuilder = new ComparationBlockBuilder();
            builder(comparationBlockBuilder);
            script.Filters.AddLast(comparationBlockBuilder.ComparationBlock);
            return new SelectFromTable<TTable>(table, script, databaseManager);
        }
    }
}
