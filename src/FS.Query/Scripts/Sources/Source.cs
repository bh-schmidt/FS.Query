using FS.Query.Settings;

namespace FS.Query.Scripts.Sources
{
    public abstract class Source : ISource
    {
        private string? treatedAlias;

        public Source(string alias)
        {
            Alias = alias;
        }

        public string Alias { get; }
        public string TreatedAlias
        {
            get
            {
                 return treatedAlias ??= $"[{Alias}]";
            }
        }
        public abstract object Build(DbSettings dbSettings);
    }
}
