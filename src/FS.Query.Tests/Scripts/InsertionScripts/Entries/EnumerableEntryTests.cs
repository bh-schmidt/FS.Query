using FS.Query.Scripts.Columns;
using FS.Query.Scripts.InsertionScripts.Entries;
using FS.Query.Settings;
using FS.Query.Settings.Conversions;
using FS.Query.Tests.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FS.Query.Tests.Scripts.InsertionScripts.Entries
{
    public class EnumerableEntryTests
    {
        EnumerableEntry enumerableEntry = null!;
        Mock<DbSettings> dbSettings = new Mock<DbSettings>();

        [SetUp]
        public void Setup()
        {
            dbSettings = new();
        }

        [Test]
        public void Will_build_the_entry()
        {
            var users = new List<User>
            {
                new User{IsActive = true, Name = "User 1"},
                new User{IsActive = true, Name = "User 2"},
                new User{IsActive = true, Name = "User 3"},
            };

            var columns = new List<IAliasColumn>
            {
                new AliasTableColumn("u", typeof(User), "IsActive"),
                new AliasTableColumn("u", typeof(User), "Name")
            };

            dbSettings.Setup(e => e.TypeMapping)
                .Returns(new TypeMapping());

            enumerableEntry = new EnumerableEntry(typeof(User), users, dbSettings.Object, columns);

            var result = enumerableEntry.Build(dbSettings.Object);

            Assert.NotNull(result);
            Assert.AreEqual("(1, 'User 1'), (1, 'User 2'), (1, 'User 3')", result.ToString());
        }

        [Test]
        public void Will_build_the_entry_with_named_column()
        {
            var users = new List<User>
            {
                new User{IsActive = true, Name = "User 1"},
                new User{IsActive = true, Name = "User 2"},
                new User{IsActive = true, Name = "User 3"},
            };

            var columns = new List<IAliasColumn>
            {
                new AliasColumn("u", "IsActive"),
                new AliasColumn("u", "Name")
            };

            dbSettings.Setup(e => e.TypeMapping)
                .Returns(new TypeMapping());

            enumerableEntry = new EnumerableEntry(typeof(User), users, dbSettings.Object, columns);

            var result = enumerableEntry.Build(dbSettings.Object);

            Assert.NotNull(result);
            Assert.AreEqual("(1, 'User 1'), (1, 'User 2'), (1, 'User 3')", result.ToString());
        }

        [Test]
        public void Will_throw_null_type()
        {
            var users = new List<User>
            {
                new User{IsActive = true, Name = "User 1"},
                new User{IsActive = true, Name = "User 2"},
                new User{IsActive = true, Name = "User 3"},
            };

            var columns = new List<IAliasColumn>
            {
                new AliasTableColumn("u", typeof(User), "IsActive"),
                new AliasTableColumn("u", typeof(User), "Name")
            };

            Assert.Throws<ArgumentException>(() =>
            {
                new EnumerableEntry(null!, users, dbSettings.Object, columns);
            }, "The type can't be null.");
        }

        [Test]
        public void Will_throw_null_values()
        {
            var columns = new List<IAliasColumn>
            {
                new AliasTableColumn("u", typeof(User), "IsActive"),
                new AliasTableColumn("u", typeof(User), "Name")
            };

            dbSettings.Setup(e => e.TypeMapping)
                .Returns(new TypeMapping());

            enumerableEntry = new EnumerableEntry(typeof(User), null!, dbSettings.Object, columns);

            Assert.Throws<ArgumentException>(() =>
            {
                enumerableEntry.Build(dbSettings.Object);
            }, "The values can't be null or empty.");
        }

        [Test]
        public void Will_throw_empty_values()
        {
            var users = new List<User> { };

            var columns = new List<IAliasColumn>
            {
                new AliasTableColumn("u", typeof(User), "IsActive"),
                new AliasTableColumn("u", typeof(User), "Name")
            };

            dbSettings.Setup(e => e.TypeMapping)
                .Returns(new TypeMapping());

            enumerableEntry = new EnumerableEntry(typeof(User), users, dbSettings.Object, columns);

            Assert.Throws<ArgumentException>(() =>
            {
                enumerableEntry.Build(dbSettings.Object);
            }, "The values can't be null or empty.");
        }

        [Test]
        public void Will_throw_null_columns()
        {
            var users = new List<User>
            {
                new User{IsActive = true, Name = "User 1"},
                new User{IsActive = true, Name = "User 2"},
                new User{IsActive = true, Name = "User 3"},
            };

            Assert.Throws<ArgumentException>(() =>
                new EnumerableEntry(typeof(User), users, dbSettings.Object, null!)
                , "The values can't be null or empty.");
        }

        [Test]
        public void Will_throw_empty_columns()
        {
            var users = new List<User>
            {
                new User{IsActive = true, Name = "User 1"},
                new User{IsActive = true, Name = "User 2"},
                new User{IsActive = true, Name = "User 3"},
            };
            var columns = new List<IAliasColumn> { };

            Assert.Throws<ArgumentException>(() =>
                new EnumerableEntry(typeof(User), users, dbSettings.Object, columns),
                "The values can't be null or empty.");
        }

        [Test]
        public void Will_throw_inexistent_property()
        {
            var users = new List<User>
            {
                new User{IsActive = true, Name = "User 1"},
                new User{IsActive = true, Name = "User 2"},
                new User{IsActive = true, Name = "User 3"},
            };
            var columns = new List<IAliasColumn>
            {
                new AliasColumn("u", "InexistentColumn1"),
                new AliasColumn("u", "InexistentColumn2")
            };

            dbSettings.Setup(e => e.TypeMapping)
                .Returns(new TypeMapping());

            Assert.Throws<ArgumentException>(
                () => new EnumerableEntry(typeof(User), users, dbSettings.Object, columns),
                "The column/property informed does not exist in the object type.");
        }
    }
}
