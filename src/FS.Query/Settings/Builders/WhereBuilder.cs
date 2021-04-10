using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Operators;
using System.Linq;
using System.Text;

namespace FS.Query.Settings.Builders
{
    public class WhereBuilder
    {
        public virtual object? Build(DbSettings dbSettings, SelectionScript selectionScript)
        {
            if (selectionScript.Filters.Count == 0)
                return null;

            var builder = new StringBuilder();
            builder.Append(" WHERE ");

            var filters = selectionScript.Filters.ToArray();

            int index = 0;
            foreach (var filter in filters)
            {
                var blockBuilder = filter.Build(dbSettings);
                builder.Append(blockBuilder);

                if (++index < filters.Length)
                    builder.Append($" {Operator.And.Operator} ");
            }

            return builder;
        }
    }
}
