using FS.Query.Scripts.Columns;
using FS.Query.Scripts.InsertionScripts;
using FS.Query.Scripts.SelectionScripts.Sources;
using FS.Query.Settings;
using FS.Query.Tests.Shared;
using Moq;
using NUnit.Framework;
using System;

namespace FS.Query.Tests.Scripts.InsertionScripts
{
    public class InsertionScriptTests
    {
        InsertionScript insertionScript = null!;
        Mock<Table> table = null!;
        Mock<DbSettings> dbSettings = null!;

        [SetUp]
        public void Setup()
        {
            table = new Mock<Table>(typeof(User), "u");
            dbSettings = new Mock<DbSettings>();
        }

        [Test]
        public void Will_build_the_script()
        {
            insertionScript = new InsertionScript(table.Object);
            table.Setup(e => e.Build(dbSettings.Object))
                .Returns("dbo.User");

            insertionScript.Columns.AddLast(new Column("Id"));
            insertionScript.Columns.AddLast(new Column("Name"));

            var result = insertionScript.Build(dbSettings.Object);

            Assert.NotNull(result);
            Assert.AreEqual("INSERT INTO dbo.User ([Id], [Name]) VALUES @entry", result.BuildedScript);
        }

        [Test]
        public void Will_throw_null_table()
        {
            insertionScript = new InsertionScript(null!);

            Assert.Throws<ArgumentException>(
                () => insertionScript.Build(dbSettings.Object),
                "Table can't be null");
        }

        [Test]
        public void Will_throw_null_builded_table()
        {
            insertionScript = new InsertionScript(table.Object);
            table.Setup(e => e.Build(dbSettings.Object))
                .Returns(null);

            Assert.Throws<Exception>(
                () => insertionScript.Build(dbSettings.Object),
                "The builded table can't be null.");
        }
    }
}
