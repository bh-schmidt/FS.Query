using System;
using System.Text;

namespace FS.Query.Factory.Filters
{
    public class ComparationBlock : Comparable
    {
        public ComparationNode? ComparationNode { get; set; }

        public override StringBuilder Build()
        {
            if (ComparationNode is null)
                throw new ArgumentException("The block can't be empty.");

            var nodeBuilder = ComparationNode.Build();

            return new StringBuilder()
                .Append('(')
                .Append(nodeBuilder)
                .Append(')');
        }
    }
}
