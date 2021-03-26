using FS.Query.Mapping;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace FS.Query.Factory.Joins
{
    public static class JoinByExpression
    {
        public static StringBuilder Build(Expression expression, DbSettings dbSettings, ITypedSource firstSource, ITypedSource secondSource, ParameterExpression[] parametersExpressions)
        {
            try
            {
                return BuildInternal(expression, dbSettings, firstSource, secondSource, parametersExpressions);
            }
            catch (InvalidExpressionException)
            {
                throw new InvalidExpressionException($"The expression between join {firstSource.Type.Name} and {secondSource.Type.Name} is invalid.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static StringBuilder BuildInternal(Expression expression, DbSettings dbSettings, ITypedSource firstSource, ITypedSource secondSource, ParameterExpression[] parametersExpressions)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                var dictionary = new Dictionary<string, (ITypedSource, ObjectMap)>();

                var firstMap = dbSettings.MapCaching.GetOrCreate(firstSource.Type);
                var secondMap = dbSettings.MapCaching.GetOrCreate(secondSource.Type);

                dictionary.Add(parametersExpressions[0].Name!, (firstSource, firstMap));
                dictionary.Add(parametersExpressions[1].Name!, (secondSource, secondMap));

                var stringBuilder = Mount(dictionary, binaryExpression);
                return stringBuilder;
            }

            if (expression is MemberExpression memberExpression)
            {
                if (memberExpression.Expression is MemberExpression)
                    throw new InvalidExpressionException();

                if (memberExpression.Expression is not ParameterExpression parameterExpression)
                    throw new InvalidExpressionException();

                var table = parameterExpression.Name == parametersExpressions[0].Name ?
                    firstSource :
                    secondSource;

                var map = dbSettings.MapCaching.GetOrCreate(table.Type);
                var propertyMap = map.PropertiesToColumns.GetValueOrDefault(memberExpression.Member.Name);

                if (propertyMap is null)
                    throw new Exception("The property map can't be found.");

                return new StringBuilder($"{table.TreatedAlias}.{propertyMap.TreatedColumnName} = 1");
            }

            if (expression is ConstantExpression constantExpression)
            {
                if ((bool)constantExpression.Value! == true)
                    return new StringBuilder("1 = 1");
                else
                    return new StringBuilder("1 = 0");
            }

            throw new ArgumentException($"The expression between join {firstSource.Type.Name} and {secondSource.Type.Name} is invalid.");
        }

        private static StringBuilder Mount(Dictionary<string, (ITypedSource, ObjectMap)> dictionary, Expression expression)
        {
            StringBuilder stringBuilder = new();

            if (expression is BinaryExpression binaryExpression)
            {
                stringBuilder.Append('(');

                var leftBuilder = Mount(dictionary, binaryExpression.Left);
                var rightBuilder = Mount(dictionary, binaryExpression.Right);

                AddOperator(binaryExpression.NodeType, stringBuilder, leftBuilder, rightBuilder);
                return stringBuilder.Append(')');
            }

            if (expression is MemberExpression memberExpression)
            {
                if (memberExpression.Expression is MemberExpression)
                    throw new InvalidExpressionException();

                if (memberExpression.Expression is not ParameterExpression parameterExpression)
                    throw new InvalidExpressionException();

                // prevent methods
                var tuple = dictionary!.GetValueOrDefault(parameterExpression.Name);
                var propertyMap = tuple.Item2.PropertiesToColumns.GetValueOrDefault(memberExpression.Member.Name);

                if (propertyMap is null)
                    throw new InvalidExpressionException();

                return stringBuilder
                    .Append(tuple.Item1.TreatedAlias)
                    .Append('.')
                    .Append(propertyMap.TreatedColumnName);
            }

            if (expression is ConstantExpression constantExpression)
            {
                var value = SqlConversion.ToSql(constantExpression.Type, constantExpression.Value);
                return stringBuilder.Append(value);
            }

            throw new InvalidExpressionException();
        }

        private static void AddOperator(ExpressionType expressionType, StringBuilder builder, StringBuilder leftBuilder, StringBuilder rightBuilder)
        {
            switch (expressionType)
            {
                case ExpressionType.Add:
                    AddBetween(builder, leftBuilder, "+", rightBuilder);
                    break;
                case ExpressionType.AddChecked:
                    AddBetween(builder, leftBuilder, "+", rightBuilder);
                    break;
                case ExpressionType.And:
                    AddBetween(builder, leftBuilder, "AND", rightBuilder);
                    break;
                case ExpressionType.AndAlso:
                    AddBetween(builder, leftBuilder, "AND", rightBuilder);
                    break;
                case ExpressionType.Coalesce:
                    builder
                        .Append("ISNULL(")
                        .Append(leftBuilder)
                        .Append(", ")
                        .Append(rightBuilder)
                        .Append(')');
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    break;
                case ExpressionType.Convert:
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.Divide:
                    break;
                case ExpressionType.Equal:
                    AddBetween(builder, leftBuilder, "=", rightBuilder);
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    AddBetween(builder, leftBuilder, ">", rightBuilder);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    AddBetween(builder, leftBuilder, ">=", rightBuilder);
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                    AddBetween(builder, leftBuilder, "<=", rightBuilder);
                    break;
                case ExpressionType.LessThanOrEqual:
                    AddBetween(builder, leftBuilder, "<=", rightBuilder);
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    AddBetween(builder, leftBuilder, "%", rightBuilder);
                    break;
                case ExpressionType.Multiply:
                    AddBetween(builder, leftBuilder, "*", rightBuilder);
                    break;
                case ExpressionType.MultiplyChecked:
                    AddBetween(builder, leftBuilder, "*", rightBuilder);
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    AddBetween(builder, leftBuilder, "<>", rightBuilder);
                    break;
                case ExpressionType.Or:
                    AddBetween(builder, leftBuilder, "OR", rightBuilder);
                    break;
                case ExpressionType.OrElse:
                    AddBetween(builder, leftBuilder, "OR", rightBuilder);
                    break;
                case ExpressionType.Parameter:
                    break;
                case ExpressionType.Power:
                    AddBetween(builder, leftBuilder, "^", rightBuilder);
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.Subtract:
                    AddBetween(builder, leftBuilder, "-", rightBuilder);
                    break;
                case ExpressionType.SubtractChecked:
                    AddBetween(builder, leftBuilder, "-", rightBuilder);
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.Assign:
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Throw:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.Unbox:
                    break;
                case ExpressionType.AddAssign:
                    break;
                case ExpressionType.AndAssign:
                    break;
                case ExpressionType.DivideAssign:
                    break;
                case ExpressionType.ExclusiveOrAssign:
                    break;
                case ExpressionType.LeftShiftAssign:
                    break;
                case ExpressionType.ModuloAssign:
                    break;
                case ExpressionType.MultiplyAssign:
                    break;
                case ExpressionType.OrAssign:
                    break;
                case ExpressionType.PowerAssign:
                    break;
                case ExpressionType.RightShiftAssign:
                    break;
                case ExpressionType.SubtractAssign:
                    break;
                case ExpressionType.AddAssignChecked:
                    break;
                case ExpressionType.MultiplyAssignChecked:
                    break;
                case ExpressionType.SubtractAssignChecked:
                    break;
                case ExpressionType.PreIncrementAssign:
                    break;
                case ExpressionType.PreDecrementAssign:
                    break;
                case ExpressionType.PostIncrementAssign:
                    break;
                case ExpressionType.PostDecrementAssign:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.OnesComplement:
                    break;
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.IsFalse:
                    break;
                default:
                    throw new ArgumentException($"Invalid expression. The expression type can't be converted to an operator.");
            }

        }

        private static void AddBetween(StringBuilder builder, StringBuilder leftBuilder, string @operator, StringBuilder rightBuilder) =>
            builder.Append(leftBuilder)
                .Append(' ')
                .Append(@operator)
                .Append(' ')
                .Append(rightBuilder);
    }
}
