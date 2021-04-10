using FS.Query.Settings;

namespace FS.Query.Scripts.SelectionScripts.Sources
{
    public interface ISource
    {
        string Alias { get; }
        string TreatedAlias { get; }

        object Build(DbSettings dbSettings);
    }
}
