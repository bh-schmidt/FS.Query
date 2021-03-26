using FS.Query.Settings;
using System.Collections.Generic;
using System.Text;

namespace FS.Query.Factory
{
    public interface ISource
    {
        string Alias { get; }
        string TreatedAlias { get; }
        LinkedList<string> PropertiesToSelect { get; }

        object GetSourceId();
        StringBuilder Build(DbSettings dbSettings);
    }
}
