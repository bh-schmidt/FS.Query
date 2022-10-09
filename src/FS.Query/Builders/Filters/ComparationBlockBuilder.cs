using FS.Query.Helpers.Extensions;
using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Filters;
using System;
using System.Linq.Expressions;
using FS.Query.Scripts.Columns;

namespace FS.Query.Builders.Filters
{
    public class ComparationBlockBuilder
    {
        private readonly SelectionScript selectionScript;

        private ComparationNode? lasNode;
        private LogicalConnectiveBuilder? logicalConnective;
        private ComparationBlock? comparationBlock;

        public ComparationBlockBuilder(SelectionScript selectionScript)
        {
            this.selectionScript = selectionScript;
        }

        public virtual LogicalConnectiveBuilder LogicalConnective { get => logicalConnective ??= new(this); }
        public virtual ComparationBlock ComparationBlock { get => comparationBlock ??= new(); }
        public virtual ComparationNode? LastNode
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
            var column = new AliasTableColumn(tableAlias, typeof(TTable), propInfo.Name);
            return new EqualityBuilder(selectionScript, this, LogicalConnective, column);
        }
    }
}
