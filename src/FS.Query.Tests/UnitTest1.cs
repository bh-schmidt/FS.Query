using FS.Query.Settings;
using FS.Query.Settings.Mapping;
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
            var settings = DbSettingsBuilder.Create()
                .WithConnection(e => new SqlConnection("Server=localhost;Database=FsDb;User Id=sa;Password=Test@123;"))
                .Map<PersonMap>()
                .Build();

            var dbManager = new DbManager(settings, null);

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

            var bytes1 = GC.GetTotalMemory(true);
            dbManager
                .FromTable<Person>("p1")
                .Select(e => e.Id, e => e.FullName!)

                //.Join<Person>("p2", (p1, p2) => p1.Id == p2.Id)
                //.Select(e => e.Id, e => e.BirthDay!)

                //.Where(builder =>
                //{
                //    builder
                //        .Column<Person>("p1", p => p.Id).Equals(Guid.NewGuid(), false);
                //})
                .Execute<Person>();
            var bytes2 = GC.GetTotalMemory(true);


            var diff1 = bytes2 - bytes1;
        }

        [Test]
        public void Test()
        {
        }
    }

    public class Person
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public string? FullName { get; set; }
        public DateTime? BirthDay { get; set; }

        public Person? Father { get; set; }
    }

    public class PersonMap : Map<Person>
    {
        public PersonMap()
        {
            Property(t => t.Id);
            Property(t => t.IsActive).WithName("Active");
            Property(t => t.FullName).WithName("Name");
            Property(t => t.BirthDay);
        }
    }
}