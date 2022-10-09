using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Settings;
using FS.Query.Scripts.Columns;

namespace FS.Query.Scripts.SelectionScripts.Parameters
{
    public abstract class SqlParameter : ISqlComparable
    {
        private readonly ScriptParameters scriptParameters;

        public bool IsConstant { get; }

        public SqlParameter(ScriptParameters scriptParameters, bool isConstant)
        {
            this.scriptParameters = scriptParameters;
            IsConstant = isConstant;
        }

        public object BuildWithAlias(DbSettings dbSettings)
        {
            if (IsConstant || GetType() == typeof(ComparationBlock))
                return BuildAsString(dbSettings);

            return scriptParameters.Add(this);
        }

        public abstract object BuildAsString(DbSettings dbSettings);
    }
}
