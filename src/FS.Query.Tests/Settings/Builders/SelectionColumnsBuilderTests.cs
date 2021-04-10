using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Selects;
using FS.Query.Settings;
using FS.Query.Settings.Builders;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace FS.Query.Tests.Settings.Builders
{
    public class SelectionColumnsBuilderTests
    {
        readonly SelectionColumnsBuilder selectionColumnsBuilder = new();
        Mock<SelectionScript> selectionScript = null!;
        Mock<Select> select = null!;
        Mock<IScriptColumn> column = null!;

        [SetUp]
        public void Setup()
        {
            selectionScript = new(null);
            select = new(null, false);
            column = new();
        }

        [Test]
        public void Will_build_the_columns()
        {
            column.Setup(e => e.Build(It.IsAny<DbSettings>()))
                .Returns("{COLUMN}");

            var columns = new LinkedList<IScriptColumn>();
            columns.AddLast(column.Object);

            select.SetupGet(e => e.Columns)
                .Returns(columns);

            var selects = new LinkedList<Select>();
            selects.AddLast(select.Object);

            selectionScript.SetupGet(e => e.Selects)
                .Returns(selects);

            var result = selectionColumnsBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.NotNull(result);
            Assert.AreEqual(" {COLUMN}", result.ToString());
            column.Verify(e => e.Build(It.IsAny<DbSettings>()), Times.Once);
            select.Verify(e => e.Columns, Times.Once);
            selectionScript.Verify(e => e.Selects, Times.Exactly(2));
        }

        [Test]
        public void Will_build_two_columns()
        {
            column.Setup(e => e.Build(It.IsAny<DbSettings>()))
                .Returns("{COLUMN}");

            var columns = new LinkedList<IScriptColumn>();
            columns.AddLast(column.Object);
            columns.AddLast(column.Object);

            select.SetupGet(e => e.Columns)
                .Returns(columns);

            var selects = new LinkedList<Select>();
            selects.AddLast(select.Object);

            selectionScript.SetupGet(e => e.Selects)
                .Returns(selects);

            var result = selectionColumnsBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.NotNull(result);
            Assert.AreEqual(" {COLUMN}, {COLUMN}", result.ToString());
            column.Verify(e => e.Build(It.IsAny<DbSettings>()), Times.Exactly(2));
            select.Verify(e => e.Columns, Times.Once);
            selectionScript.Verify(e => e.Selects, Times.Exactly(2));
        }

        [Test]
        public void Will_build_the_select_everything_of_two_sources()
        {
            select.SetupSequence(e => e.TableAlias)
                .Returns("T1")
                .Returns("T2");

            select.Setup(e => e.SelectEverything)
                .Returns(true);

            var selects = new LinkedList<Select>();
            selects.AddLast(select.Object);
            selects.AddLast(select.Object);

            selectionScript.SetupGet(e => e.Selects)
                .Returns(selects);

            var result = selectionColumnsBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.NotNull(result);
            Assert.AreEqual(" [T1].*, [T2].*", result.ToString());
            select.Verify(e => e.Columns, Times.Never);
            selectionScript.Verify(e => e.Selects, Times.Exactly(2));
        }

        [Test]
        public void Will_select_everything_because_there_is_no_selections()
        {
            var selects = new LinkedList<Select>();

            selectionScript.SetupGet(e => e.Selects)
                .Returns(selects);

            var result = selectionColumnsBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.NotNull(result);
            Assert.AreEqual("*", result.ToString());
            selectionScript.Verify(e => e.Selects, Times.Once);
        }
    }
}
