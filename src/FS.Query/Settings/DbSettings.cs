using FS.Query.Settings.Caching;

namespace FS.Query.Settings
{
    public class DbSettings
    {
        public virtual Connection Connection { get; set; } = null!;
        public virtual MapCaching MapCaching { get; set; } = null!;
        public virtual ScriptCaching ScriptCache { get; set; } = null!;
        public virtual TypeMap TypeMap { get; set; } = null!;
    }
}
