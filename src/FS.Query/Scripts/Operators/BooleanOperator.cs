using System;

namespace FS.Query.Scripts.Operators
{
    public class BooleanOperator : MiddleOperator
    {
        public BooleanOperator(string @operator) : base(@operator) { }

        public override bool Equals(object? obj) => obj is BooleanOperator @operator && Operator == @operator.Operator;
        public override int GetHashCode() => HashCode.Combine(Operator);
    }
}
