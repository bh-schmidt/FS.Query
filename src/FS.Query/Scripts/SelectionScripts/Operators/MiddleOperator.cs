using FS.Query.Scripts.SelectionScripts.Filters.Comparables;
using FS.Query.Settings;

namespace FS.Query.Scripts.SelectionScripts.Operators
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
