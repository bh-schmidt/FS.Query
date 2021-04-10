using FS.Query.Settings;

namespace FS.Query.Scripts.SelectionScripts.Sources
{
    public class ScriptInjection : Source
    {
        private readonly string injection;

        public ScriptInjection(string alias, string injection) : base(alias)
        {
            this.injection = injection;
        }

        public override object Build(DbSettings dbSettings)
        {
            return $"({injection}) [{Alias}]";
        }
    }
}
