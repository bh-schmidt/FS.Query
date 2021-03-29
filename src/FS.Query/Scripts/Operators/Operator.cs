using FS.Query.Scripts.Filters;
using FS.Query.Settings;
using System.Text;
using FS.Query.Scripts.Filters.Comparables;

namespace FS.Query.Scripts.Operators
{
    public abstract class Operator
    {
        public abstract object Build(DbSettings dbSettings, ISqlComparable first, ISqlComparable second);

        public static LogicalConnectiveOperator And => new("AND");
        public static LogicalConnectiveOperator Or => new("OR");
        public static LogicalConnectiveOperator Xor => new("XOR");
        public static BooleanOperator Equal => new("=");
        public static BooleanOperator Different => new("<>");
        public static BooleanOperator GreaterThan => new(">");
        public static BooleanOperator GreaterThanOrEqual => new(">=");
        public static BooleanOperator LessThan => new("<");
        public static BooleanOperator LessThanOrEqual => new("<=");
    }
}
