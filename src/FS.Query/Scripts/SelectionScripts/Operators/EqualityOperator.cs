using System;

namespace FS.Query.Scripts.SelectionScripts.Operators
{
    public class EqualityOperator : MiddleOperator
    {
        public EqualityOperator(string @operator) : base(@operator) { }

        public override bool Equals(object? obj) => obj is EqualityOperator @operator && Operator == @operator.Operator;
        public override int GetHashCode() => HashCode.Combine(Operator);
    }
}
