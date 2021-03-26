using FS.Query.Mapping;
using FS.Query.Settings;
using NUnit.Framework;
using System;
using System.Data.SqlClient;

namespace FS.Query.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Exe()
        {
            var settings = DbSettingsBuilder.Create()
                .WithConnection(e => new SqlConnection("Server=localhost;Database=FsDb;User Id=sa;Password=Test@123;"))
                .Map<PersonMap>()
                .Build();

            var databaseManager = new DatabaseManager(settings, null);

            databaseManager
                .FromTable<Person>("p1")
                .Select(e => e.Id)
                .Select(e => e.Name!)
                .Join<Person>("p2", (p1, p2) => p1.Id == p2.Id)
                .Select(e => e.Id)
                .Select(e => e.BirthDay!)
                .Where(builder =>
                {
                    builder.Column<Person>("p1", p => p.Id);

                })
                .Execute<Person>();
        }
    }

    public class Person
    {
        public long Id { get; set; }
        public bool Active { get; set; }
        public string? Name { get; set; }
        public DateTime? BirthDay { get; set; }

        public Person? Father { get; set; }
    }

    public class PersonMap : Map<Person>
    {
        public PersonMap()
        {
            Property(t => t.Id);
            Property(t => t.Active);
            Property(t => t.Name);
            Property(t => t.BirthDay);
        }
    }
}