using FS.Query.Settings;

namespace FS.Query.Scripts.Sources
{
    public interface ISource
    {
        string Alias { get; }
        string TreatedAlias { get; }

        object Build(DbSettings dbSettings);
    }
}
