using FS.Query.Helpers.Extensions;
using FS.Query.Scripts;
using FS.Query.Scripts.Filters;
using FS.Query.Scripts.Selects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FS.Query.Builders.Selects
{
    public class SelectBuilder<T>
    {
        private readonly SelectionScript script;

        public SelectBuilder(
            SelectionScript script)
        {
            this.script = script;
        }

        public SerializeBuilder<T> Columns<TSource>(string tableAlias, params Expression<Func<TSource, object?>>[] expressions)
        {
            var propertyHierarchy = new List<Select>(expressions.Length);
            var scriptColumns = new LinkedList<IScriptColumn>();

            for (int i = 0; i < expressions.Length; i++)
            {
                var expression = expressions[i];
                var propertyInfo = expression.GetPropertyInfo();
                var column = new TableProperty(tableAlias, typeof(T), propertyInfo.Name);
                scriptColumns.AddLast(column);
            }

            var select = new Select(tableAlias, scriptColumns);
            propertyHierarchy.Add(select);
            script.Selects.AddLast(select);
            return new SerializeBuilder<T>(propertyHierarchy);
        }
    }
}
