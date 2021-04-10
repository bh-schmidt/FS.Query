using FS.Query.Scripts.SelectionScripts.Sources;
using FS.Query.Settings;
using System;
using System.Linq.Expressions;
using System.Text;

namespace FS.Query.Scripts.SelectionScripts.Combinations.Joins
{
    public class ExpressionJoin : Combination
    {
        private readonly ParameterExpression[] parameterExpressions;
        private readonly Expression expression;

        public ExpressionJoin(
            Source firstSource,
            Source secondSource,
            ParameterExpression[] parameterExpressions,
            Expression expressionBody) : base(firstSource, secondSource)
        {
            if (parameterExpressions.Length != 2) throw new ArgumentException($"{nameof(parameterExpressions)} can't have a lenght different from 2.");

            this.parameterExpressions = parameterExpressions;
            expression = expressionBody;
        }

        public string GetExpressionString() => expression.ToString();

        public override object Build(DbSettings dbSettings)
        {
            var buildedExpression = JoinByExpression.Build(expression, dbSettings, (ITypedSource)FirstSource, (ITypedSource)SecondSource, parameterExpressions);
            var buildedSource = SecondSource.Build(dbSettings);

            return new StringBuilder("JOIN ")
                .Append(buildedSource)
                .Append(" ON ")
                .Append(buildedExpression);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not ExpressionJoin join)
                return false;

            return
                join.FirstSource == FirstSource &&
                join.SecondSource == SecondSource &&
                join.GetExpressionString() == GetExpressionString();
        }

        public override int GetHashCode() =>
            HashCode.Combine(FirstSource, SecondSource, GetExpressionString());
    }
}
