using FS.Query.Scripts.Sources;

namespace FS.Query.Builders
{
    public class SelectionByScriptInjectionBuilder : SelectionBuilder
    {
        private readonly ScriptInjection scriptInjection;

        public SelectionByScriptInjectionBuilder(ScriptInjection scriptInjection, SelectionScript script, DbManager dbManager) : base(scriptInjection, script, dbManager)
        {
            this.scriptInjection = scriptInjection;
        }
    }
}
