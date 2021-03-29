using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.Filters;
using FS.Query.Scripts.Operators;
using System;
using System.Linq.Expressions;
using FS.Query.Scripts.Filters.Comparables;

namespace FS.Query.Builders.Filters
{
    public class EqualityBuilder
    {
        private readonly Script script;
        private readonly ComparationBlockBuilder comparationBlockBuilder;
        private readonly LogicalConnectiveBuilder logicalConnectiveBuilder;
        private readonly ISqlComparable fistComparable;

        public EqualityBuilder(Script script, ComparationBlockBuilder comparationBlockBuilder, LogicalConnectiveBuilder logicalConnectiveBuilder, ISqlComparable fistComparable)
        {
            this.script = script;
            this.comparationBlockBuilder = comparationBlockBuilder;
            this.logicalConnectiveBuilder = logicalConnectiveBuilder;
            this.fistComparable = fistComparable;
        }

        public LogicalConnectiveBuilder Equals<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.Equal);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder Equals(object value, bool isConstant = false)
        {
            var comparableValue = new ComparableValue(value, script.ScriptParameters, isConstant);
            var node = new ComparationNode(fistComparable, Operator.Equal, comparableValue);
            SetNode(node);

            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder Different<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.Different);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder Greater<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.GreaterThan);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder GreaterOrEqual<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.GreaterThanOrEqual);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder Less<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.LessThan);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder LessOrEqual<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.LessThanOrEqual);
            return logicalConnectiveBuilder;
        }

        private void AddOperator<TTable>(string alias, Expression<Func<TTable, object?>> getProperty, BooleanOperator booleanOperator)
        {
            var propInfo = getProperty.GetPropertyInfo();
            var property = new TableProperty(alias, typeof(TTable), propInfo.Name);
            var node = new ComparationNode(fistComparable, booleanOperator, property);

            SetNode(node);
        }

        private void SetNode(ComparationNode node)
        {
            if (comparationBlockBuilder.LasNode is not null)
                comparationBlockBuilder.LasNode.NextNode = node;

            comparationBlockBuilder.LasNode = node;
        }
    }
}
