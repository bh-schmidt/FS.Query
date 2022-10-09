using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using FS.Query.Scripts.Columns;

namespace FS.Query.Scripts.SelectionScripts.Filters
{
    public class ComparationBlock : ISqlComparable
    {
        public ComparationNode? ComparationNode { get; set; }

        public virtual object BuildWithAlias(DbSettings dbSettings)
        {
            if (ComparationNode is null)
                throw new ArgumentException("The comparation block can't be empty.");

            var buildedNode = ComparationNode.Build(dbSettings);

            return new StringBuilder()
                .Append('(')
                .Append(buildedNode)
                .Append(')');
        }

        public override bool Equals(object? obj) => obj is ComparationBlock block && EqualityComparer<ComparationNode?>.Default.Equals(ComparationNode, block.ComparationNode);
        public override int GetHashCode() => HashCode.Combine(ComparationNode);
    }
}
