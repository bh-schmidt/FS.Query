using FS.Query.Settings;

namespace FS.Query.Scripts.Filters.Comparables
{
    public interface ISqlComparable
    {
        object Build(DbSettings dbSettings);
    }
}
