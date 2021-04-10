using FS.Query.Scripts.SelectionScripts;
using System.Text;

namespace FS.Query.Settings.Builders
{
    public class FromBuilder
    {
        public virtual object Build(DbSettings dbSettings, SelectionScript selectionScript)
        {
            var from = selectionScript.From.Build(dbSettings);
            return new StringBuilder(" FROM ")
                .Append(from);
        }
    }
}
