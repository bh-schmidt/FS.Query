using FS.Query.Factory;
using FS.Query.Factory.Joins;
using FS.Query.Settings;
using System;
using System.Linq.Expressions;
using System.Text;

namespace FS.Query
{
    public class Join
    {
        private string? buildedExpression;
        private readonly ParameterExpression[] parameterExpressions;
        private readonly Expression expression;
        private readonly JoinBy joinBy;

        public Join(Source firstSource, Source secondSource, ParameterExpression[] parameterExpressions, Expression expressionBody)
        {
            if (parameterExpressions.Length != 2) throw new ArgumentException($"{nameof(parameterExpressions)} can't have a lenght different from 2.");

            FirstSource = firstSource;
            SecondSource = secondSource;
            this.parameterExpressions = parameterExpressions;
            expression = expressionBody;
            joinBy = JoinBy.Expression;
        }

        public Source FirstSource { get; }
        public Source SecondSource { get; }

        public string GetExpressionString() => expression.ToString();

        public StringBuilder Build(DbSettings dbSettings)
        {
            var stringBuilder = new StringBuilder("JOIN ")
                .Append(SecondSource.Build(dbSettings))
                .Append(" ON ");

            var buildedExpression = joinBy switch
            {
                JoinBy.Expression => JoinByExpression.Build(expression, dbSettings, (ITypedSource)FirstSource, (ITypedSource)SecondSource, parameterExpressions),
                _ => throw new ArgumentException("The 'joinBy' is invalid."),
            };

            return stringBuilder
                .Append(buildedExpression);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Join join)
                return false;

            return
                join.FirstSource.GetSourceId() == FirstSource.GetSourceId() &&
                join.SecondSource.GetSourceId() == SecondSource.GetSourceId() &&
                join.GetExpressionString() == GetExpressionString();
        }

        public override int GetHashCode() =>
            HashCode.Combine(FirstSource.GetSourceId(), SecondSource.GetSourceId(), GetExpressionString());
    }
}
