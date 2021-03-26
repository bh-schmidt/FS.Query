using FS.Query.Factory.Filters;
using System.Text;

namespace FS.Query.Scripts.Operators
{
    public abstract class Operator
    {
        public abstract StringBuilder Build(Comparable first, Comparable second);

        public static BooleanOperator And() => new BooleanOperator("AND");
        public static BooleanOperator Or() => new BooleanOperator("OR");
        public static BooleanOperator Xor() => new BooleanOperator("XOR");
        public static BooleanOperator Equal() => new BooleanOperator("=");
        public static BooleanOperator Different() => new BooleanOperator("<>");
        public static BooleanOperator GreaterThan() => new BooleanOperator(">");
        public static BooleanOperator GreaterThanOrEqual() => new BooleanOperator(">=");
        public static BooleanOperator LessThan() => new BooleanOperator("<");
        public static BooleanOperator LessThanOrEqual() => new BooleanOperator("<=");
    }
}
