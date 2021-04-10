using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.SelectionScripts.Selects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FS.Query.Builders.Selections
{
    public class SerializeBuilder<T>
    {
        private readonly List<Select> selects;

        public SerializeBuilder(List<Select> selects)
        {
            this.selects = selects;
        }

        public void PutInto(Expression<Func<T, object?>> expression)
        {
            var propertyInfos = expression.GetPropertyInfos();
            var propertyHierarchy = propertyInfos.Select(e => e.Name).ToArray();

            foreach (var select in selects)
                select.PropertyHierarchy = propertyHierarchy;
        }
    }
}
