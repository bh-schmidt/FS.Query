using FS.Query.Caching;
using System.Collections.Generic;
using System.Linq;

namespace FS.Query.Builders
{
    public static class CacheKeyBuilder
    {
        public static CacheKey Fact(Script script)
        {
            LinkedList<string> properties = new();

            Fill(script, properties);

            return new CacheKey(script.From.GetSourceId(), script.Joins.ToArray(), properties.ToArray());
        }

        private static void Fill(Script script, LinkedList<string> properties)
        {
            foreach (var property in script.From.PropertiesToSelect)
            {
                properties.AddLast(script.From.Alias);
                properties.AddLast(property);
            }

            foreach (var join in script.Joins)
                foreach (var property in join.SecondSource.PropertiesToSelect)
                {
                    properties.AddLast(join.FirstSource.TreatedAlias);
                    properties.AddLast(property);
                }
        }
    }
}
