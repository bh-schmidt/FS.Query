using FS.Query.Scripts.SelectionScripts;
using System.Text;

namespace FS.Query.Settings.Builders
{
    public class JoinBuilder
    {
        public virtual object? Build(DbSettings dbSettings, SelectionScript selectionScript)
        {
            if (selectionScript.Combinations.Count == 0)
                return null;

            var builder = new StringBuilder();

            foreach (var join in selectionScript.Combinations)
                builder
                    .Append(' ')
                    .Append(join.Build(dbSettings));

            return builder;
        }
    }
}
