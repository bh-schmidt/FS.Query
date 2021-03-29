using FS.Query.Settings.Caching;

namespace FS.Query.Settings
{
    public class DbSettings
    {
        internal MapCaching MapCaching { get; set; } = null!;
        internal ScriptCaching ScriptCache { get; set; } = null!;
        internal Connection Connection { get; set; } = null!;
    }
}
