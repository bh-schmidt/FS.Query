using FS.Query.Scripts.Sources;
using FS.Query.Settings;
using System.Text;

namespace FS.Query.Scripts.Combinations
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
