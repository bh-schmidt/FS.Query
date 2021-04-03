using FS.Query.Settings;
using FS.Query.Settings.Mapping;
using FS.Query.Tests.Shared;
using NUnit.Framework;
using System;
using System.Data.SqlClient;

namespace FS.Query.Tests
{
    public class Tests
    {
        [Test]
        public void Exe()
        {
            //var settings = DbSettingsBuilder.Create()
            //    .WithConnection(e => new SqlConnection("Server=localhost;Database=FsDb;User Id=sa;Password=Test@123;"))
            //    .Map<UserMap>()
            //    .Map<UserPostMap>()
            //    .Build();

            //var dbManager = new DbManager(settings, null);

            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

            //var bytes1 = GC.GetTotalMemory(true);
            //var persons = dbManager
            //    .FromTable<User>("u")
            //    .Join<UserPost>("p", (u, p) => u.Id == p.UserId)
            //    .Where(builder =>
            //    {
            //        builder
            //            .Column<User>("u", p => p.Id).NotIn(new[] { 2, 3 });
            //    })
            //    .Limit(10000)
            //    .Select<UserPost>(builder =>
            //    {
            //        builder.Columns<UserPost>("p", e => e.Id, e => e.Post);
            //        builder.Columns<User>("u", e => e.BirthDay).PutInto(e => e.User);
            //    })
            //    //.Select<User>()
            //    .GetArray();
            //var bytes2 = GC.GetTotalMemory(true);

            //var diff = bytes2 - bytes1;
        }

        [Test]
        public void Test()
        {
        }
    }
}