using FS.Query.Settings;

namespace FS.Query.Scripts.Columns
{
    public interface IColumn
    {
        string ColumnName { get; }
        string TreatedColumnName { get; }

        object Build(DbSettings dbSettings);
    }
}
