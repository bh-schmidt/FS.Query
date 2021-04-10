using FS.Query.Settings.Builders;
using FS.Query.Settings.Caching;
using FS.Query.Settings.Conversions;

namespace FS.Query.Settings
{
    public class DbSettings
    {
        public virtual Connection Connection { get; set; } = null!;
        public virtual ObjectMapCaching MapCaching { get; set; } = null!;
        public virtual ScriptCaching ScriptCache { get; set; } = null!;
        public virtual TypeMapping TypeMapping { get; set; } = null!;
        public virtual ScriptBuilder ScriptBuilder { get; set; } = null!;
    }
}
