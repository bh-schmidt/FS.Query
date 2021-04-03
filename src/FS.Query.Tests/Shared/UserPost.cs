namespace FS.Query.Tests.Shared
{
    public class UserPost
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? Post { get; set; }

        public User? User { get; set; }
    }
}
