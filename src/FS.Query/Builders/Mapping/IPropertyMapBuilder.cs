namespace FS.Query.Builders.Mapping
{
    public interface IPropertyMapBuilder<TProperty>
    {
        PropertyMapBuilder<TProperty> WithName(string databaseName);
    }
}
