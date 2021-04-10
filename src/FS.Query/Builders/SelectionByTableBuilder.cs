using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Combinations.Joins;
using FS.Query.Scripts.SelectionScripts.Sources;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FS.Query.Builders
{
    public class SelectionByTableBuilder<TTable> : SelectionBuilder
    {
        private readonly SelectionScript selectionScript;
        private readonly Table table;
        private readonly DbManager dbManager;

        public SelectionByTableBuilder(Table table, SelectionScript selectionScript, DbManager dbManager) : base(table, selectionScript, dbManager)
        {
            this.selectionScript = selectionScript;
            this.table = table;
            this.dbManager = dbManager;
        }

        public SelectionByTableBuilder<TJoinType> Join<TJoinType>(string alias, Expression<Func<TTable, TJoinType, bool>> where)
        {
            var joinTable = new Table(typeof(TJoinType), alias);
            var join = new ExpressionJoin(table, joinTable, where.Parameters.ToArray(), where.Body);
            selectionScript.Combinations.AddLast(join);
            return new SelectionByTableBuilder<TJoinType>(joinTable, selectionScript, dbManager);
        }
    }
}
