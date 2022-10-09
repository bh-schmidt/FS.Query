using FS.Query.Settings.Mapping;

namespace FS.Query.Tests.Shared
{
    public class UserPostMap : TableMap<UserPost>
    {
        public UserPostMap()
        {
            TableName("User_Post");

            Property(t => t.Id);
            Property(t => t.UserId).WithName("User_Id");
            Property(t => t.Post);
        }
    }
}
