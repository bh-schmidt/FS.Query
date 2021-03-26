using FS.Query.Scripts.Operators;
using System;
using System.Text;

namespace FS.Query.Factory.Filters
{
    public class ComparationNode
    {
        public ComparationNode(Comparable first, Operator @operator, Comparable second, BooleanOperator? booleanOperator = null, ComparationNode? nextNode = null)
        {
            if (booleanOperator is null ^ nextNode is null)
                throw new ArgumentException($"When setting an {nameof(@operator)} the {nameof(nextNode)} is required and vice-versa.");

            First = first;
            Operator = @operator;
            Second = second;
            BooleanOperator = booleanOperator;
            NextNode = nextNode;
        }

        public Comparable First { get; set; }
        public Operator Operator { get; set; }
        public Comparable Second  { get; set; }
        public BooleanOperator? BooleanOperator { get; }
        public ComparationNode? NextNode { get; }

        public StringBuilder Build()
        {
            var stringBuilder = Operator.Build(First, Second);

            if(BooleanOperator is not null)
                return NextNode!.Build()
                    .Append($" {BooleanOperator.Operator} ")
                    .Append(stringBuilder);

            return stringBuilder;
        }
    }
}
