using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Selects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FS.Query.Scripts.Columns;

namespace FS.Query.Builders.Selections
{
    public class SelectionScriptBuilder<T>
    {
        private readonly SelectionScript selectionScript;

        public SelectionScriptBuilder(
            SelectionScript selectionScript)
        {
            this.selectionScript = selectionScript;
        }

        public SerializeBuilder<T> Columns<TTable>(string tableAlias, params Expression<Func<TTable, object?>>[] expressions)
        {
            var propertyHierarchy = new List<Select>(expressions.Length);
            var scriptColumns = new LinkedList<IAliasColumn>();

            for (int i = 0; i < expressions.Length; i++)
            {
                var expression = expressions[i];
                var propertyInfo = expression.GetPropertyInfo();
                var column = new AliasTableColumn(tableAlias, typeof(TTable), propertyInfo.Name);
                scriptColumns.AddLast(column);
            }

            var select = new Select(tableAlias, scriptColumns);
            propertyHierarchy.Add(select);
            selectionScript.Selects.AddLast(select);
            return new SerializeBuilder<T>(propertyHierarchy);
        }
    }
}
