using FS.Query.Factory.Filters;
using System.Text;

namespace FS.Query.Scripts.Operators
{
    public class MiddleOperator : Operator
    {
        public MiddleOperator(string @operator)
        {
            Operator = @operator;
        }

        public string Operator { get; }

        public override StringBuilder Build(Comparable first, Comparable second)
        {
            return new StringBuilder($"{first.Build()} {Operator} {second.Build()}");
        }
    }
}
