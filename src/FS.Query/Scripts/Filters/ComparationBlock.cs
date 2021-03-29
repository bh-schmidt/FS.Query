using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using FS.Query.Scripts.Filters.Comparables;

namespace FS.Query.Scripts.Filters
{
    public class ComparationBlock : ISqlComparable
    {
        public ComparationNode? ComparationNode { get; set; }

        public object Build(DbSettings dbSettings)
        {
            if (ComparationNode is null)
                throw new ArgumentException("The block can't be empty.");

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
