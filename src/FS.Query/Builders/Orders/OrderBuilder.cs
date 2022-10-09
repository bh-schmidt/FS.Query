using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Scripts.SelectionScripts.Orders;
using System;
using System.Linq.Expressions;
using FS.Query.Scripts.Columns;

namespace FS.Query.Builders.Orders
{
    public class OrderBuilder
    {
        private readonly SelectionScript selectionScript;

        public OrderBuilder(SelectionScript selectionScript)
        {
            this.selectionScript = selectionScript;
        }

        public OrderBuilder Column<TTable>(string tableAlias, Expression<Func<TTable, object?>> property, bool descending = false)
        {
            var propertyInfo = property.GetPropertyInfo();
            var column = new AliasTableColumn(tableAlias, typeof(TTable), propertyInfo.Name);
            var columnOrder = new ColumnOrder(column, descending);
            selectionScript.Orders.AddLast(columnOrder);
            return this;
        }
    }
}
