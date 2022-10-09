using FS.Query.Settings;

namespace FS.Query.Scripts.SelectionScripts.Sources
{
    public abstract class Source : ISource
    {
        public Source(string alias)
        {
            Alias = alias;
        }

        public string Alias { get; }

        public string TreatedAlias => Alias ?? $"[{Alias}]";

        public abstract object Build(DbSettings dbSettings);
    }
}
