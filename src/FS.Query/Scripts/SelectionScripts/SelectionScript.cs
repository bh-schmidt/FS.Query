using FS.Query.Scripts.SelectionScripts.Combinations;
using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Scripts.SelectionScripts.Orders;
using FS.Query.Scripts.SelectionScripts.Parameters;
using FS.Query.Scripts.SelectionScripts.Selects;
using FS.Query.Scripts.SelectionScripts.Sources;
using FS.Query.Settings;
using System.Collections.Generic;
using System.Linq;

namespace FS.Query.Scripts.SelectionScripts
{
    public class SelectionScript
    {
        private LinkedList<Select>? selects;
        private LinkedList<Combination>? combinations;
        private LinkedList<ComparationBlock>? filters;
        private ScriptParameters? scriptParameters;
        private LinkedList<ColumnOrder>? orders;

        public SelectionScript(Source from) => From = from;

        public virtual Source From { get; }
        public virtual LinkedList<Select> Selects => selects ??= new();
        public virtual LinkedList<Combination> Combinations => combinations ??= new();
        public virtual LinkedList<ComparationBlock> Filters => filters ??= new();
        public virtual LinkedList<ColumnOrder> Orders => orders ??= new();
        public virtual ScriptParameters ScriptParameters => scriptParameters ??= new();
        public virtual long? Limit { get; set; }

        public virtual BuildedSelectionScript Build(DbSettings dbSettings)
        {
            var script = dbSettings.ScriptBuilder.Build(dbSettings, this);
            return new BuildedSelectionScript(script, Selects.ToArray(), ScriptParameters);
        }

        public virtual SelectionCacheKey GetKey() => new(From, Combinations.ToArray(), Selects.ToArray(), Filters.ToArray(), Limit ?? 0);
    }
}
