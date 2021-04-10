using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Scripts.SelectionScripts.Orders;
using System;
using System.Linq.Expressions;

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
            var tableProperty = new TableProperty(tableAlias, typeof(TTable), propertyInfo.Name);
            var columnOrder = new ColumnOrder(tableProperty, descending);
            selectionScript.Orders.AddLast(columnOrder);
            return this;
        }
    }
}
