using FS.Query.Scripts.SelectionScripts;
using FS.Query.Settings.Builders;
using Moq;
using NUnit.Framework;
using System;

namespace FS.Query.Tests.Settings.Builders
{
    public class LimitBuilderTests
    {
        readonly LimitBuilder limitBuilder = new();
        Mock<SelectionScript> selectionScript = null!;

        [SetUp]
        public void Setup()
        {
            selectionScript = new(null);
        }

        [Test]
        public void Will_add_the_limit()
        {
            selectionScript.SetupGet(e => e.Limit)
                .Returns(5);

            var result = limitBuilder.Build(selectionScript.Object);

            Assert.NotNull(result);
            selectionScript.Verify(e => e.Limit, Times.Exactly(3));
            Assert.AreEqual(" TOP 5", result!.ToString());
        }

        [Test]
        public void Will_throw_limit_lower_than_1()
        {
            selectionScript.SetupGet(e => e.Limit)
                .Returns(0);

            Assert.Throws<Exception>(() => limitBuilder.Build(selectionScript.Object), "Limit can't be lower than 1");
        }


        [Test]
        public void Will_return_empty_because_there_is_no_limit_filter()
        {
            var result = limitBuilder.Build(selectionScript.Object);
            Assert.Null(result);
        }
    }
}
