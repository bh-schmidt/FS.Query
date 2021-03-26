using FS.Query.Caching;

namespace FS.Query.Settings
{
    public class DbSettings
    {
        public MapCaching MapCaching { get; set; }
        public ScriptCaching ScriptCache { get; set; }
        public Connection Connection { get; set; }

        public DbSettings()
        {

        }
    }
}
