using FS.Query.Scripts.Filters.Comparables;
using FS.Query.Settings;

namespace FS.Query.Scripts.Parameters
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

        public object Build(DbSettings dbSettings)
        {
            if (IsConstant)
                return BuildParameter(dbSettings);

            return scriptParameters.Add(this);
        }

        public abstract object BuildParameter(DbSettings dbSettings);
    }
}
