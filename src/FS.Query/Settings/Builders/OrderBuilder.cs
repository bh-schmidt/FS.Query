using FS.Query.Scripts.SelectionScripts;
using System.Text;

namespace FS.Query.Settings.Builders
{
    public class OrderBuilder
    {
        public virtual object? Build(DbSettings dbSettings, SelectionScript selectionScript)
        {
            if (selectionScript.Orders.Count == 0)
                return null;

            var builder = new StringBuilder();
            builder.Append(" ORDER BY ");

            var addComma = false;
            foreach (var order in selectionScript.Orders)
            {
                if (addComma)
                    builder.Append($", ");
                else
                    addComma = true;

                builder.Append(order.ScriptColumn.Build(dbSettings));

                if (order.Descending)
                    builder.Append(" DESC");
            }

            return builder;
        }
    }
}
