using FS.Query.Settings;

namespace FS.Query.Scripts.SelectionScripts.Filters.Comparables
{
    public interface ISqlComparable
    {
        object Build(DbSettings dbSettings);
    }
}
