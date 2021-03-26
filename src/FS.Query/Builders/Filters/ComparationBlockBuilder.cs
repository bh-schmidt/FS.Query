using FS.Query.Factory.Filters;
using System;
using System.Linq.Expressions;

namespace FS.Query.Builders.Filters
{
    public class ComparationBlockBuilder
    {
        public ComparationBlock ComparationBlock { get; set; } = new ComparationBlock();

        public ComparationBlockBuilder() { }

        public void Column<TTable>(string tableAlias, Expression<Func<TTable, object>> getProperty)
        {
        }
    }
}
