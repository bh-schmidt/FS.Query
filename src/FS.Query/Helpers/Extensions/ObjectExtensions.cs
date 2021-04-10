using System.Text;

namespace FS.Query.Helpers.Extensions
{
    public static class ObjectExtensions
    {
        public static StringBuilder SurroundByQuotes(this object value) =>
            new StringBuilder()
                .Append('\'')
                .Append(value)
                .Append('\'');
    }
}
