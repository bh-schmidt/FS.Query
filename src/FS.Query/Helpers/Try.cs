using System;

namespace FS.Query.Helpers
{
    internal static class Try
    {
        internal static void Execute(Action action)
        {
            try
            {
                action();
            }
            catch { }
        }
    }
}
