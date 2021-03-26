using System;

namespace FS.Query.Factory
{
    public interface ITypedSource : ISource
    {
        public Type Type { get; }
    }
}
