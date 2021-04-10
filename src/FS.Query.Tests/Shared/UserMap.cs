using FS.Query.Settings.Mapping;

namespace FS.Query.Tests.Shared
{
    public class UserMap : TableMap<User>
    {
        public UserMap()
        {
            Property(t => t.Id);
            Property(t => t.IsActive).WithName("Active");
            Property(t => t.Name);
            Property(t => t.BirthDay);
        }
    }
}
