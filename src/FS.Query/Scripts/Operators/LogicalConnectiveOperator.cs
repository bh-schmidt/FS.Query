using System;

namespace FS.Query.Scripts.Operators
{
    public class LogicalConnectiveOperator : MiddleOperator
    {
        public LogicalConnectiveOperator(string @operator) : base(@operator) { }

        public override bool Equals(object? obj) => obj is LogicalConnectiveOperator @operator && Operator == @operator.Operator;
        public override int GetHashCode() => HashCode.Combine(Operator);
    }
}
