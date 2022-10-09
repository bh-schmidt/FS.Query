using FS.Query.Settings;

namespace FS.Query.Scripts.Columns
{
    public interface ISqlComparable
    {
        object BuildWithAlias(DbSettings dbSettings);
    }
}
