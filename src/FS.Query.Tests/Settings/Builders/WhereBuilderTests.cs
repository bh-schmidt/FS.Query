using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Settings;
using FS.Query.Settings.Builders;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace FS.Query.Tests.Settings.Builders
{
    public class WhereBuilderTests
    {
        readonly WhereBuilder whereBuilder = new();
        Mock<SelectionScript> selectionScript = null!;
        Mock<ComparationBlock> comparationBlock = null!;

        [SetUp]
        public void Setup()
        {
            selectionScript = new(null);
            comparationBlock = new();
        }

        [Test]
        public void Will_build_the_where()
        {
            var comparations = new LinkedList<ComparationBlock>();
            comparations.AddLast(comparationBlock.Object);

            selectionScript.SetupGet(e => e.Filters)
                .Returns(comparations);

            comparationBlock.Setup(e => e.Build(It.IsAny<DbSettings>()))
                .Returns("{COMPARATION}");

            var result = whereBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.NotNull(result);
            Assert.AreEqual(" WHERE {COMPARATION}", result!.ToString());
            selectionScript.Verify(e => e.Filters, Times.Exactly(2));
            comparationBlock.Verify(e => e.Build(It.IsAny<DbSettings>()), Times.Once);
        }

        [Test]
        public void Will_build_the_where_with_two_comparations()
        {
            var comparations = new LinkedList<ComparationBlock>();
            comparations.AddLast(comparationBlock.Object);
            comparations.AddLast(comparationBlock.Object);

            selectionScript.SetupGet(e => e.Filters)
                .Returns(comparations);

            comparationBlock.Setup(e => e.Build(It.IsAny<DbSettings>()))
                .Returns("{COMPARATION}");

            var result = whereBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.NotNull(result);
            Assert.AreEqual(" WHERE {COMPARATION} AND {COMPARATION}", result!.ToString());
            selectionScript.Verify(e => e.Filters, Times.Exactly(2));
            comparationBlock.Verify(e => e.Build(It.IsAny<DbSettings>()), Times.Exactly(2));
        }

        [Test]
        public void Wont_build_because_there_is_no_comparations()
        {
            var comparations = new LinkedList<ComparationBlock>();

            selectionScript.SetupGet(e => e.Filters)
                .Returns(comparations);

            var result = whereBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.Null(result);
            selectionScript.Verify(e => e.Filters, Times.Once);
        }
    }
}
