using System;
using System.Linq.Expressions;

namespace FS.Query.Builders.Filters
{
    public class EqualityBuilder
    {
        public EqualityBuilder()
        {

        }
        public void IsEqualsTo<TTable>(string alias, Expression<Func<TTable, object>> getProperty)
        {

        }
    }
}
