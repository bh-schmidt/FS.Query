using System;

namespace FS.Query.Scripts.Sources
{
    public interface ITypedSource : ISource
    {
        public Type Type { get; }
    }
}
