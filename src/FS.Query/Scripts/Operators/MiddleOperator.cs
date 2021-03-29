using FS.Query.Scripts.Filters;
using FS.Query.Settings;
using System.Text;
using FS.Query.Scripts.Filters.Comparables;

namespace FS.Query.Scripts.Operators
{
    public class MiddleOperator : Operator
    {
        public MiddleOperator(string @operator)
        {
            Operator = @operator;
        }

        public string Operator { get; }

        public override object Build(DbSettings dbSettings, ISqlComparable first, ISqlComparable second) =>
            $"{first.Build(dbSettings)} {Operator} {second.Build(dbSettings)}";
    }
}
