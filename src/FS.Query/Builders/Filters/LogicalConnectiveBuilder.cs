using FS.Query.Scripts.Operators;

namespace FS.Query.Builders.Filters
{
    public class LogicalConnectiveBuilder
    {
        private readonly ComparationBlockBuilder comparationBlockBuilder;

        public LogicalConnectiveBuilder(ComparationBlockBuilder comparationBlockBuilder)
        {
            this.comparationBlockBuilder = comparationBlockBuilder;
        }

        public ComparationBlockBuilder And()
        {
            if (comparationBlockBuilder.LasNode is not null)
                comparationBlockBuilder.LasNode.LogicalConnective = Operator.And;

            return comparationBlockBuilder;
        }

        public ComparationBlockBuilder Or()
        {
            if (comparationBlockBuilder.LasNode is not null)
                comparationBlockBuilder.LasNode.LogicalConnective = Operator.Or;

            return comparationBlockBuilder;
        }

        public ComparationBlockBuilder Xor()
        {
            if (comparationBlockBuilder.LasNode is not null)
                comparationBlockBuilder.LasNode.LogicalConnective = Operator.Xor;

            return comparationBlockBuilder;
        }
    }
}
