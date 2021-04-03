using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.Filters;
using FS.Query.Scripts.Orders;
using System;
using System.Linq.Expressions;

namespace FS.Query.Builders.Orders
{
    public class OrderBuilder
    {
        private readonly SelectionScript script;

        public OrderBuilder(SelectionScript script)
        {
            this.script = script;
        }

        public OrderBuilder Column<TTable>(string tableAlias, Expression<Func<TTable, object?>> property, bool descending = false)
        {
            var propertyInfo = property.GetPropertyInfo();
            var tableProperty = new TableProperty(tableAlias, typeof(TTable), propertyInfo.Name);
            var columnOrder = new ColumnOrder(tableProperty, descending);
            script.Orders.AddLast(columnOrder);
            return this;
        }
    }
}
