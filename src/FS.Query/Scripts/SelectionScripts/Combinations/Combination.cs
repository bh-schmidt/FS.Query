using FS.Query.Scripts.SelectionScripts.Sources;
using FS.Query.Settings;

namespace FS.Query.Scripts.SelectionScripts.Combinations
{
    public abstract class Combination
    {
        public Combination(ISource firstSource, ISource secondSource)
        {
            FirstSource = firstSource;
            SecondSource = secondSource;
        }

        public ISource FirstSource { get; }
        public ISource SecondSource { get; }

        public abstract object Build(DbSettings dbSettings);
    }
}
