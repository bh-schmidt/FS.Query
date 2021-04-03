using FS.Query.Scripts.Joins;
using FS.Query.Scripts.Sources;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FS.Query.Builders
{
    public class SelectionByTableBuilder<TTable> : SelectionBuilder
    {
        private readonly SelectionScript script;
        private readonly Table table;
        private readonly DbManager dbManager;

        public SelectionByTableBuilder(Table table, SelectionScript script, DbManager dbManager) : base(table, script, dbManager)
        {
            this.script = script;
            this.table = table;
            this.dbManager = dbManager;
        }

        public SelectionByTableBuilder<TJoinType> Join<TJoinType>(string alias, Expression<Func<TTable, TJoinType, bool>> where)
        {
            var joinTable = new Table(typeof(TJoinType), alias);
            var join = new ExpressionJoin(table, joinTable, where.Parameters.ToArray(), where.Body);
            script.Combinations.AddLast(join);
            return new SelectionByTableBuilder<TJoinType>(joinTable, script, dbManager);
        }
    }
}
