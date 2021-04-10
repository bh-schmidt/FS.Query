using FS.Query.Scripts.SelectionScripts;
using System;

namespace FS.Query.Settings.Builders
{
    public class LimitBuilder
    {
        public virtual object? Build(SelectionScript selectionScript)
        {
            if (selectionScript.Limit is null)
                return null;

            if (selectionScript.Limit < 1)
                throw new Exception("Limit can't be lower than 1");

            return $" TOP {selectionScript.Limit}";
        }
    }
}
