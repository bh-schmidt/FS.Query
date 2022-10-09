using FS.Query.Settings;
using FS.Query.Tests.Shared;
using Moq;
using NUnit.Framework;
using System.Data;

namespace FS.Query.Tests.Flows
{
    public class InsertionTests
    {
        DbManager dbManager = null!;
        Mock<IDbConnection> dbConnection = null!;
        Mock<IDbDataParameter> dbDataParameter = null!;

        [SetUp]
        public void Setup()
        {
            dbConnection = new();
            dbDataParameter = new();

            var settings = DbSettingsBuilder.Create()
                .WithConnection(e => dbConnection.Object)
                .Map<UserMap>()
                .Map<UserPostMap>()
                .Build();

            dbManager = new DbManager(settings, null!);
        }

        [Test]
        public void Will_insert_one()
        {
            var user = new User
            {
                Name = "User 1",
                BirthDay = new System.DateTime(1990, 01, 01)
            };

            dbManager.Insert<User>()
                .Columns(e => e.Name, e => e.BirthDay)
                .Value(user);
        }
    }
}
