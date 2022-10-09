using FS.Query.Scripts.Columns;
using FS.Query.Scripts.InsertionScripts;
using FS.Query.Scripts.InsertionScripts.Entries;
using FS.Query.Settings;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace FS.Query.Tests.Scripts.InsertionScripts
{
    public class BuildedInsertionScriptTests
    {
        BuildedInsertionScript buildedInsertionScript = null!;
        Mock<DbSettings> dbSettings = null!;
        Mock<Entry> entry = null!;

        [SetUp]
        public void Setup()
        {
            dbSettings = new();
            var columns = new IColumn[] { new Column("Id") }.AsEnumerable();
            entry = new(columns);
        }

        [Test]
        public void Will_build_the_script()
        {
            var columns = new IColumn[] { new Column("Id") };

            entry.Setup(e => e.Build(dbSettings.Object))
                .Returns("{ENTRY}");

            buildedInsertionScript = new("{INSERT} @entry", columns);

            var result = buildedInsertionScript.Build(dbSettings.Object, entry.Object);

            Assert.NotNull(result);
            Assert.AreEqual("{INSERT} {ENTRY}", result);
        }

        [Test]
        public void Will_throw_empty_script()
        {
            var columns = new IColumn[] { new Column("Id") };

            Assert.Throws<ArgumentException>(
                () => new BuildedInsertionScript("", columns),
                "The buildedScript cannot be null or empty.");
        }

        [Test]
        public void Will_throw_empty_columns()
        {
            Assert.Throws<ArgumentException>(
                () => new BuildedInsertionScript("{INSERT} @entry", null!),
                "The buildedScript cannot be null or empty.");
        }

        [Test]
        public void Will_throw_null_entry()
        {
            var columns = new IColumn[] { new Column("Id") };

            buildedInsertionScript = new("{INSERT} @entry", columns);

            Assert.Throws<ArgumentException>(
                () => buildedInsertionScript.Build(dbSettings.Object, null!),
                "Entry can't be null");
        }

        [Test]
        public void Will_throw_null_builded_entry()
        {
            var columns = new IColumn[] { new Column("Id") };

            entry.Setup(e => e.Build(dbSettings.Object))
                .Returns(null);

            buildedInsertionScript = new("{INSERT} @entry", columns);

            Assert.Throws<Exception>(
                () => buildedInsertionScript.Build(dbSettings.Object, entry.Object),
                "The builded entry can't be null.");
        }
    }
}
