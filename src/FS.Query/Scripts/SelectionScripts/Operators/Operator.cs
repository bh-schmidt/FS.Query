using FS.Query.Settings;
using FS.Query.Scripts.Columns;

namespace FS.Query.Scripts.SelectionScripts.Operators
{
    public abstract class Operator
    {
        public abstract object Build(DbSettings dbSettings, ISqlComparable first, ISqlComparable second);

        public static LogicalConnectiveOperator And => new("AND");
        public static LogicalConnectiveOperator Or => new("OR");
        public static LogicalConnectiveOperator Xor => new("XOR");
        public static EqualityOperator Equal => new("=");
        public static EqualityOperator NotEqual => new("<>");
        public static EqualityOperator In => new("IN");
        public static EqualityOperator NotIn => new("NOT IN");
        public static EqualityOperator GreaterThan => new(">");
        public static EqualityOperator GreaterThanOrEqual => new(">=");
        public static EqualityOperator LessThan => new("<");
        public static EqualityOperator LessThanOrEqual => new("<=");
    }
}
