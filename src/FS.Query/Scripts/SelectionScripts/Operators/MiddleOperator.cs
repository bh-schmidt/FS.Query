using FS.Query.Settings;
using FS.Query.Scripts.Columns;

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
            $"{first.BuildWithAlias(dbSettings)} {Operator} {second.BuildWithAlias(dbSettings)}";
    }
}
