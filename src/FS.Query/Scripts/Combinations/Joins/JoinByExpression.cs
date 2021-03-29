using FS.Query.Scripts.Sources;
using FS.Query.Settings;
using FS.Query.Settings.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace FS.Query.Scripts.Joins
{
    public static class JoinByExpression
    {
        public static object Build(Expression expression, DbSettings dbSettings, ITypedSource firstSource, ITypedSource secondSource, ParameterExpression[] parametersExpressions)
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

        private static object BuildInternal(Expression expression, DbSettings dbSettings, ITypedSource firstSource, ITypedSource secondSource, ParameterExpression[] parametersExpressions)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                var dictionary = new Dictionary<string, (ITypedSource, ObjectMap)>();

                var firstMap = dbSettings.MapCaching.GetOrCreate(firstSource.Type);
                var secondMap = dbSettings.MapCaching.GetOrCreate(secondSource.Type);

                dictionary.Add(parametersExpressions[0].Name!, (firstSource, firstMap));
                dictionary.Add(parametersExpressions[1].Name!, (secondSource, secondMap));

                var stringBuilder = BuildValue(dictionary, binaryExpression);
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
                var propertyMap = map.GetProperty(memberExpression.Member.Name);

                if (propertyMap is null)
                    throw new Exception("The property map can't be found.");

                return $"{table.TreatedAlias}.{propertyMap.TreatedColumnName} = 1";
            }

            if (expression is ConstantExpression constantExpression)
                return (bool)constantExpression.Value! == true ?
                     "1 = 1" :
                     "1 = 0";

            throw new ArgumentException($"The expression between join {firstSource.Type.Name} and {secondSource.Type.Name} is invalid.");
        }

        private static object BuildValue(Dictionary<string, (ITypedSource, ObjectMap)> dictionary, Expression expression)
        {

            if (expression is BinaryExpression binaryExpression)
            {
                var leftBuilder = BuildValue(dictionary, binaryExpression.Left);
                var rightBuilder = BuildValue(dictionary, binaryExpression.Right);

                var stringBuilder = new StringBuilder().Append('(');
                AddOperator(binaryExpression.NodeType, stringBuilder, leftBuilder, rightBuilder);
                return stringBuilder.Append(')');
            }

            if (expression is MemberExpression memberExpression)
            {
                if (memberExpression.Expression is MemberExpression)
                    throw new InvalidExpressionException();

                if (memberExpression.Expression is not ParameterExpression parameterExpression)
                    throw new InvalidExpressionException();

                var tuple = dictionary!.GetValueOrDefault(parameterExpression.Name);
                var propertyMap = tuple.Item2.GetProperty(memberExpression.Member.Name);

                if (propertyMap is null)
                    throw new InvalidExpressionException();

                return $"{tuple.Item1.TreatedAlias}.{propertyMap.TreatedColumnName}";
            }

            if (expression is ConstantExpression constantExpression)
                return SqlConversion.ToSql(constantExpression.Type, constantExpression.Value);

            throw new InvalidExpressionException();
        }

        private static void AddOperator(ExpressionType expressionType, StringBuilder builder, object left, object right)
        {
            switch (expressionType)
            {
                case ExpressionType.Add:
                    AddBetween(builder, left, "+", right);
                    break;
                case ExpressionType.AddChecked:
                    AddBetween(builder, left, "+", right);
                    break;
                case ExpressionType.And:
                    AddBetween(builder, left, "AND", right);
                    break;
                case ExpressionType.AndAlso:
                    AddBetween(builder, left, "AND", right);
                    break;
                case ExpressionType.Coalesce:
                    builder
                        .Append("ISNULL(")
                        .Append(left)
                        .Append(", ")
                        .Append(right)
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
                    AddBetween(builder, left, "=", right);
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    AddBetween(builder, left, ">", right);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    AddBetween(builder, left, ">=", right);
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                    AddBetween(builder, left, "<=", right);
                    break;
                case ExpressionType.LessThanOrEqual:
                    AddBetween(builder, left, "<=", right);
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    AddBetween(builder, left, "%", right);
                    break;
                case ExpressionType.Multiply:
                    AddBetween(builder, left, "*", right);
                    break;
                case ExpressionType.MultiplyChecked:
                    AddBetween(builder, left, "*", right);
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
                    AddBetween(builder, left, "<>", right);
                    break;
                case ExpressionType.Or:
                    AddBetween(builder, left, "OR", right);
                    break;
                case ExpressionType.OrElse:
                    AddBetween(builder, left, "OR", right);
                    break;
                case ExpressionType.Parameter:
                    break;
                case ExpressionType.Power:
                    AddBetween(builder, left, "^", right);
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.Subtract:
                    AddBetween(builder, left, "-", right);
                    break;
                case ExpressionType.SubtractChecked:
                    AddBetween(builder, left, "-", right);
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

        private static void AddBetween(StringBuilder builder, object left, string @operator, object right) =>
            builder.Append($"{left} {@operator} {right}");
    }
}
