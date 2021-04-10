using System;

namespace FS.Query.Scripts.SelectionScripts.Sources
{
    public interface ITypedSource : ISource
    {
        public Type Type { get; }
    }
}
