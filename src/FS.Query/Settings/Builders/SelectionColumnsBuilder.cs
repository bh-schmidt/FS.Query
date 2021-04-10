using FS.Query.Scripts.SelectionScripts;
using System.Text;

namespace FS.Query.Settings.Builders
{
    public class SelectionColumnsBuilder
    {
        public virtual object Build(DbSettings dbSettings, SelectionScript selectionScript)
        {
            if (selectionScript.Selects.Count == 0)
                return '*';

            var stringBuilder = new StringBuilder(" ");

            var addComma = false;
            foreach (var select in selectionScript.Selects)
            {
                if (select.SelectEverything)
                {
                    if (addComma) stringBuilder.Append(", ");
                    else addComma = true;

                    stringBuilder.Append($"[{select.TableAlias}].*");
                    continue;
                }

                foreach (var column in select.Columns)
                {
                    if (addComma) stringBuilder.Append(", ");
                    else addComma = true;

                    stringBuilder.Append(column.Build(dbSettings));
                }
            }

            return stringBuilder;
        }
    }
}
