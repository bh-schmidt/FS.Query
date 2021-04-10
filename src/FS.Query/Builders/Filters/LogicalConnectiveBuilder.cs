using FS.Query.Scripts.SelectionScripts.Operators;

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
            if (comparationBlockBuilder.LastNode is not null)
                comparationBlockBuilder.LastNode.LogicalConnective = Operator.And;

            return comparationBlockBuilder;
        }

        public ComparationBlockBuilder Or()
        {
            if (comparationBlockBuilder.LastNode is not null)
                comparationBlockBuilder.LastNode.LogicalConnective = Operator.Or;

            return comparationBlockBuilder;
        }

        public ComparationBlockBuilder Xor()
        {
            if (comparationBlockBuilder.LastNode is not null)
                comparationBlockBuilder.LastNode.LogicalConnective = Operator.Xor;

            return comparationBlockBuilder;
        }
    }
}
