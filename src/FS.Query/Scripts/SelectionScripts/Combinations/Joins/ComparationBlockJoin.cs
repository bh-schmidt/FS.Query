using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Scripts.SelectionScripts.Sources;
using FS.Query.Settings;
using System.Text;

namespace FS.Query.Scripts.SelectionScripts.Combinations.Joins
{
    public class ComparationBlockJoin : Combination
    {
        private readonly ComparationBlock comparationBlock;

        public ComparationBlockJoin(ISource firstSource, ISource secondSource, ComparationBlock comparationBlock) : base(firstSource, secondSource)
        {
            this.comparationBlock = comparationBlock;
        }

        public override object Build(DbSettings dbSettings)
        {
            return new StringBuilder()
                .Append("JOIN ")
                .Append(SecondSource.Build(dbSettings))
                .Append(" ON ")
                .Append(comparationBlock.BuildWithAlias(dbSettings));
        }
    }
}
