using FS.Query.Scripts.SelectionScripts;
using System.Text;

namespace FS.Query.Settings.Builders
{
    public class ScriptBuilder
    {
        public ScriptBuilder() : this(new FromBuilder(), new JoinBuilder(), new LimitBuilder(), new OrderBuilder(), new SelectionColumnsBuilder(), new WhereBuilder()) { }

        public ScriptBuilder(FromBuilder fromBuilder, JoinBuilder joinBuilder, LimitBuilder limitBuilder, OrderBuilder orderBuilder, SelectionColumnsBuilder selectionColumnsBuilder, WhereBuilder whereBuilder)
        {
            FromBuilder = fromBuilder;
            JoinBuilder = joinBuilder;
            LimitBuilder = limitBuilder;
            OrderBuilder = orderBuilder;
            SelectionColumnsBuilder = selectionColumnsBuilder;
            WhereBuilder = whereBuilder;
        }

        public FromBuilder FromBuilder { get; }
        public virtual JoinBuilder JoinBuilder { get; }
        public virtual LimitBuilder LimitBuilder { get; }
        public virtual OrderBuilder OrderBuilder { get; }
        public virtual SelectionColumnsBuilder SelectionColumnsBuilder { get; }
        public virtual WhereBuilder WhereBuilder { get; }

        public virtual string Build(DbSettings dbSettings, SelectionScript selectionScript)
        {
            var limit = LimitBuilder.Build(selectionScript);
            var columnsToSelect = SelectionColumnsBuilder.Build(dbSettings, selectionScript);
            var from = FromBuilder.Build(dbSettings, selectionScript);
            var joins = JoinBuilder.Build(dbSettings, selectionScript);
            var where = WhereBuilder.Build(dbSettings, selectionScript);
            var order = OrderBuilder.Build(dbSettings, selectionScript);

            var stringBuilder = new StringBuilder("SELECT");

            if (limit is not null)
                stringBuilder.Append(limit);

            stringBuilder.Append(columnsToSelect).Append(from);

            if (joins is not null)
                stringBuilder.Append(joins);

            if (where is not null)
                stringBuilder.Append(where);

            if (order is not null)
                stringBuilder.Append(order);

            return stringBuilder.ToString();
        }
    }
}
