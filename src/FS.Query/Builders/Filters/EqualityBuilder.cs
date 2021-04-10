using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Scripts.SelectionScripts.Filters.Comparables;
using FS.Query.Scripts.SelectionScripts.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FS.Query.Builders.Filters
{
    public class EqualityBuilder
    {
        private readonly SelectionScript selectionScript;
        private readonly ComparationBlockBuilder comparationBlockBuilder;
        private readonly LogicalConnectiveBuilder logicalConnectiveBuilder;
        private readonly ISqlComparable fistComparable;

        public EqualityBuilder(SelectionScript selectionScript, ComparationBlockBuilder comparationBlockBuilder, LogicalConnectiveBuilder logicalConnectiveBuilder, ISqlComparable fistComparable)
        {
            this.selectionScript = selectionScript;
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
            var comparableValue = new ComparableValue(value, selectionScript.ScriptParameters, isConstant);
            var node = new ComparationNode(fistComparable, Operator.Equal, comparableValue);
            SetNode(node);

            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder In<TValues>(IEnumerable<TValues> values, bool isConstant = false)
        {
            var comparable = new ComparableEnumerable(selectionScript.ScriptParameters, isConstant, values.Cast<object>());
            var node = new ComparationNode(fistComparable, Operator.In, comparable);
            SetNode(node);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder NotIn<TValues>(IEnumerable<TValues> values, bool isConstant = false)
        {
            var comparable = new ComparableEnumerable(selectionScript.ScriptParameters, isConstant, values.Cast<object>());
            var node = new ComparationNode(fistComparable, Operator.NotIn, comparable);
            SetNode(node);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder NotEqual<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.NotEqual);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder NotEqual(object value, bool isConstant = false)
        {
            var comparableValue = new ComparableValue(value, selectionScript.ScriptParameters, isConstant);
            var node = new ComparationNode(fistComparable, Operator.NotEqual, comparableValue);
            SetNode(node);

            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder Greater<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.GreaterThan);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder Greater(object value, bool isConstant = false)
        {
            var comparableValue = new ComparableValue(value, selectionScript.ScriptParameters, isConstant);
            var node = new ComparationNode(fistComparable, Operator.GreaterThan, comparableValue);
            SetNode(node);

            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder GreaterOrEqual<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.GreaterThanOrEqual);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder GreaterOrEqual(object value, bool isConstant = false)
        {
            var comparableValue = new ComparableValue(value, selectionScript.ScriptParameters, isConstant);
            var node = new ComparationNode(fistComparable, Operator.GreaterThanOrEqual, comparableValue);
            SetNode(node);

            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder Less<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.LessThan);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder Less(object value, bool isConstant = false)
        {
            var comparableValue = new ComparableValue(value, selectionScript.ScriptParameters, isConstant);
            var node = new ComparationNode(fistComparable, Operator.LessThan, comparableValue);
            SetNode(node);

            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder LessOrEqual<TTable>(string alias, Expression<Func<TTable, object?>> getProperty)
        {
            AddOperator(alias, getProperty, Operator.LessThanOrEqual);
            return logicalConnectiveBuilder;
        }

        public LogicalConnectiveBuilder LessOrEqual(object value, bool isConstant = false)
        {
            var comparableValue = new ComparableValue(value, selectionScript.ScriptParameters, isConstant);
            var node = new ComparationNode(fistComparable, Operator.LessThanOrEqual, comparableValue);
            SetNode(node);

            return logicalConnectiveBuilder;
        }

        private void AddOperator<TTable>(string alias, Expression<Func<TTable, object?>> getProperty, EqualityOperator booleanOperator)
        {
            var propInfo = getProperty.GetPropertyInfo();
            var property = new TableProperty(alias, typeof(TTable), propInfo.Name);
            var node = new ComparationNode(fistComparable, booleanOperator, property);

            SetNode(node);
        }

        private void SetNode(ComparationNode node)
        {
            if (comparationBlockBuilder.LastNode is not null)
                comparationBlockBuilder.LastNode.NextNode = node;

            comparationBlockBuilder.LastNode = node;
        }
    }
}
