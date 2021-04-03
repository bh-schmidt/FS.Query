﻿using FS.Query.Scripts.Filters;
using FS.Query.Scripts.Sources;
using FS.Query.Settings;
using System.Text;

namespace FS.Query.Scripts.Combinations.Joins
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
                .Append(comparationBlock.Build(dbSettings));
        }
    }
}
