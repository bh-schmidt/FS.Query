using FS.Query.Settings;
using FS.Query.Tests.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;

namespace FS.Query.Tests.Flows
{
    public class SelectionTests
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
        public void Will_get_all_users()
        {
            int total = 10;
            var reader = UserDataReaderFactory.Fact(total, new[] { "Id", "Name", "IsActive", "BirthDay" });

            dbConnection.Setup(e => e.CreateCommand().ExecuteReader())
                .Returns(reader);

            var users = dbManager
                .FromTable<User>("u")
                .Select<User>()
                .GetArray();

            Assert.NotNull(users);
            Assert.AreEqual(total, users.Length);
            dbConnection.VerifySet(e =>
                e.CreateCommand().CommandText = "SELECT [u].[Id], [u].[Active], [u].[Name], [u].[BirthDay] FROM [dbo].[User] [u]",
                Times.Once);
        }

        [Test]
        public void Will_get_all_users_selecting_id_and_name()
        {
            int total = 10;
            var reader = UserDataReaderFactory.Fact(total, new[] { "Id", "Name" });

            dbConnection.Setup(e => e.CreateCommand().ExecuteReader())
                .Returns(reader);

            var users = dbManager
                .FromTable<User>("u")
                .Select<User>(builder =>
                {
                    builder.Columns<User>("u", e => e.Id, e => e.Name);
                })
                .GetArray();

            Assert.NotNull(users);
            Assert.AreEqual(total, users.Length);
            dbConnection.VerifySet(e =>
                e.CreateCommand().CommandText = "SELECT [u].[Id], [u].[Name] FROM [dbo].[User] [u]",
                Times.Once);
        }

        [Test]
        public void Will_get_user_with_id_1()
        {
            int total = 1;
            var reader = UserDataReaderFactory.Fact(total, new[] { "Id", "Name", "IsActive", "BirthDay" });

            dbConnection.Setup(e => e.CreateCommand().Parameters.Add(It.IsAny<IDbDataParameter>()));

            dbConnection.Setup(e => e.CreateCommand().CreateParameter())
                .Returns(dbDataParameter.Object);

            dbConnection.Setup(e => e.CreateCommand().ExecuteReader())
                .Returns(reader);

            var users = dbManager
                .FromTable<User>("u")
                .Where(builder =>
                {
                    builder.Column<User>("u", p => p.Id).Equals(1);
                })
                .Select<User>()
                .GetArray();

            Assert.NotNull(users);
            Assert.AreEqual(total, users.Length);
            dbConnection.VerifySet(e =>
                e.CreateCommand().CommandText = "SELECT [u].[Id], [u].[Active], [u].[Name], [u].[BirthDay] FROM [dbo].[User] [u] WHERE ([u].[Id] = @parameter0)",
                Times.Once);
        }

        [Test]
        public void Will_get_user_with_id_1_and_2()
        {
            var ids = new[] { 1, 2 };
            int total = 2;
            var reader = UserDataReaderFactory.Fact(total, new[] { "Id", "Name", "IsActive", "BirthDay" });

            dbConnection.Setup(e => e.CreateCommand().ExecuteReader())
                .Returns(reader);

            var users = dbManager
                .FromTable<User>("u")
                .Where(builder =>
                {
                    builder.Column<User>("u", p => p.Id).In(ids);
                })
                .Select<User>()
                .GetArray();

            Assert.NotNull(users);
            Assert.AreEqual(total, users.Length);
            dbConnection.VerifySet(e =>
                e.CreateCommand().CommandText = "SELECT [u].[Id], [u].[Active], [u].[Name], [u].[BirthDay] FROM [dbo].[User] [u] WHERE ([u].[Id] IN (1, 2))",
                Times.Once);
        }

        [Test]
        public void Will_get_users_limited_by_10()
        {
            int total = 10;
            var reader = UserDataReaderFactory.Fact(total, new[] { "Id", "Name", "IsActive", "BirthDay" });

            dbConnection.Setup(e => e.CreateCommand().ExecuteReader())
                .Returns(reader);

            var users = dbManager
                .FromTable<User>("u")
                .Limit(10)
                .Select<User>()
                .GetArray();

            Assert.NotNull(users);
            Assert.AreEqual(total, users.Length);
            dbConnection.VerifySet(e =>
                e.CreateCommand().CommandText = "SELECT TOP 10 [u].[Id], [u].[Active], [u].[Name], [u].[BirthDay] FROM [dbo].[User] [u]",
                Times.Once);
        }

        [Test]
        public void Will_get_users_joined_with_posts()
        {
            int total = 10;
            var reader = UserDataReaderFactory.Fact(total, new[] { "Id", "Name", "UserPost.Post", "UserPost.UserId" });

            dbConnection.Setup(e => e.CreateCommand().ExecuteReader())
                .Returns(reader);

            var users = dbManager
                .FromTable<User>("u")
                .Join<UserPost>("p", (u, p) => u.Id == p.UserId)
                .Select<User>(builder =>
                {
                    builder.Columns<User>("u", e => e.Id, e => e.Name);
                    builder.Columns<UserPost>("p", e => e.Post, e => e.UserId).PutInto(e => e.UserPost);
                })
                .GetArray();

            Assert.NotNull(users);
            Assert.AreEqual(total, users.Length);
            Assert.NotNull(users[0].UserPost);
            dbConnection.VerifySet(e =>
                e.CreateCommand().CommandText = "SELECT [u].[Id], [u].[Name], [p].[Post], [p].[User_Id] FROM [dbo].[User] [u] JOIN [dbo].[User_Post] [p] ON ([u].[Id] = [p].[User_Id])",
                Times.Once);
        }
    }
}
