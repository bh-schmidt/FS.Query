using FS.Query.Settings.Mapping;
using FS.Query.Builders.Mapping;

namespace FS.Query.Tests.Shared
{
    public class UserPostMap : TableMap<UserPost>
    {
        public UserPostMap()
        {
            TableName("USER_POST");

            Property(t => t.Id);
            Property(t => t.UserId).WithName("USER_ID");
            Property(t => t.Post);
        }
    }
}
