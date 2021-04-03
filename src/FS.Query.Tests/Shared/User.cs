using System;

namespace FS.Query.Tests.Shared
{
    public class User
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public string? Name { get; set; }
        public DateTime BirthDay { get; set; }

        public UserPost? UserPost { get; set; }
    }
}
