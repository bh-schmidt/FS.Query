using FS.Query.Settings;
using System.Collections.Generic;
using System.Text;

namespace FS.Query.Factory
{
    public abstract class Source : ISource
    {
        public Source(string alias)
        {
            Alias = alias;
            TreatedAlias = $"[{alias}]";
        }

        public string Alias { get; }
        public string TreatedAlias { get; }
        public LinkedList<string> PropertiesToSelect { get; } = new LinkedList<string>();

        public abstract object GetSourceId();
        public abstract StringBuilder Build(DbSettings dbSettings);
    }
}
