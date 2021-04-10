using FS.Query.Scripts.SelectionScripts.Filters.Comparables;
using FS.Query.Scripts.SelectionScripts.Operators;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Query.Scripts.SelectionScripts.Filters
{
    public class ComparationNode
    {
        public ComparationNode(ISqlComparable first, Operator @operator, ISqlComparable second)
        {
            First = first;
            Operator = @operator;
            Second = second;
        }

        public virtual ISqlComparable First { get; set; }
        public virtual Operator Operator { get; set; }
        public virtual ISqlComparable Second { get; set; }
        public virtual LogicalConnectiveOperator LogicalConnective { get; set; } = Operator.And;
        public virtual ComparationNode? NextNode { get; set; }

        public object Build(DbSettings dbSettings)
        {
            var buildedOperator = Operator.Build(dbSettings, First, Second);
            SetDbType();

            if (NextNode is not null)
            {
                var buildedNode = NextNode.Build(dbSettings);
                return new StringBuilder()
                    .Append(buildedOperator)
                    .Append($" {LogicalConnective!.Operator} ")
                    .Append(buildedNode);
            }

            return buildedOperator;
        }

        public override bool Equals(object? obj)
        {
            return obj is ComparationNode node &&
                   EqualityComparer<ISqlComparable>.Default.Equals(First, node.First) &&
                   EqualityComparer<Operator>.Default.Equals(Operator, node.Operator) &&
                   EqualityComparer<ISqlComparable>.Default.Equals(Second, node.Second) &&
                   EqualityComparer<LogicalConnectiveOperator>.Default.Equals(LogicalConnective, node.LogicalConnective) &&
                   EqualityComparer<ComparationNode?>.Default.Equals(NextNode, node.NextNode);
        }

        public override int GetHashCode() =>
            HashCode.Combine(First, Operator, Second, LogicalConnective, NextNode);

        private void SetDbType()
        {
            if (Second is ComparableValue comparableValue && First is TableProperty tableProperty)
                comparableValue.DbType = tableProperty.DbType;
        }
    }
}
