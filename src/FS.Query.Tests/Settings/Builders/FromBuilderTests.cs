using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Sources;
using FS.Query.Settings;
using FS.Query.Settings.Builders;
using Moq;
using NUnit.Framework;

namespace FS.Query.Tests.Settings.Builders
{
    public class FromBuilderTests
    {
        readonly FromBuilder fromBuilder = new();
        Mock<Source> source = null!;
        Mock<SelectionScript> selectionScript = null!;

        [SetUp]
        public void Setup()
        {
            source = new(null);
            selectionScript = new(null);
        }

        [Test]
        public void Will_build_the_from()
        {
            selectionScript.SetupGet(e => e.From)
                .Returns(source.Object);

            source.Setup(e => e.Build(It.IsAny<DbSettings>()))
                .Returns("TABLE");

            var result = fromBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.AreEqual(" FROM TABLE", result.ToString());
        }
    }
}
