using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.Filters;
using FS.Query.Settings.Mapping;
using System;
using System.Linq.Expressions;

namespace FS.Query.Builders.Filters
{
    public class ComparationBlockBuilder
    {
        private readonly Script script;

        private ComparationNode? lasNode;
        private LogicalConnectiveBuilder? logicalConnective;
        private ComparationBlock? comparationBlock;

        public ComparationBlockBuilder(Script script)
        {
            this.script = script;
        }

        public LogicalConnectiveBuilder LogicalConnective { get => logicalConnective ??= new(this); }
        public ComparationBlock ComparationBlock { get => comparationBlock ??= new(); }
        public ComparationNode? LasNode
        {
            get => lasNode;
            set
            {
                lasNode = value;
                ComparationBlock.ComparationNode ??= value;
            }
        }

        public EqualityBuilder Column<TTable>(string tableAlias, Expression<Func<TTable, object?>> getProperty)
        {
            var propInfo = getProperty.GetPropertyInfo();
            var property = new TableProperty(tableAlias, typeof(TTable), propInfo.Name);
            return new EqualityBuilder(script, this, LogicalConnective, property);
        }
    }
}
