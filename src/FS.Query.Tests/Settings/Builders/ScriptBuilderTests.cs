using FS.Query.Scripts.SelectionScripts;
using FS.Query.Settings;
using FS.Query.Settings.Builders;
using Moq;
using NUnit.Framework;

namespace FS.Query.Tests.Settings.Builders
{

    public class ScriptBuilderTests
    {
        ScriptBuilder scriptBuilder = null!;
        Mock<FromBuilder> fromBuilder = null!;
        Mock<JoinBuilder> joinBuilder = null!;
        Mock<LimitBuilder> limitBuilder = null!;
        Mock<OrderBuilder> orderBuilder = null!;
        Mock<SelectionColumnsBuilder> columnsToSelectBuilder = null!;
        Mock<WhereBuilder> whereBuilder = null!;

        [SetUp]
        public void Setup()
        {
            fromBuilder = new();
            joinBuilder = new();
            limitBuilder = new();
            orderBuilder = new();
            columnsToSelectBuilder = new();
            whereBuilder = new();

            scriptBuilder = new ScriptBuilder(fromBuilder.Object, joinBuilder.Object, limitBuilder.Object, orderBuilder.Object, columnsToSelectBuilder.Object, whereBuilder.Object);
        }

        [Test]
        public void Will_build_the_script()
        {
            fromBuilder.Setup(e => e.Build(It.IsAny<DbSettings>(), It.IsAny<SelectionScript>()))
                .Returns(" {FROM}");
            joinBuilder.Setup(e => e.Build(It.IsAny<DbSettings>(), It.IsAny<SelectionScript>()))
                .Returns(" {JOIN}");
            limitBuilder.Setup(e => e.Build(It.IsAny<SelectionScript>()))
                .Returns(" {LIMIT}");
            orderBuilder.Setup(e => e.Build(It.IsAny<DbSettings>(), It.IsAny<SelectionScript>()))
                .Returns(" {ORDER}");
            columnsToSelectBuilder.Setup(e => e.Build(It.IsAny<DbSettings>(), It.IsAny<SelectionScript>()))
                .Returns(" {COLUMNS}");
            whereBuilder.Setup(e => e.Build(It.IsAny<DbSettings>(), It.IsAny<SelectionScript>()))
                .Returns(" {WHERE}");

            var result = scriptBuilder.Build(It.IsAny<DbSettings>(), It.IsAny<SelectionScript>());

            Assert.AreEqual("SELECT {LIMIT} {COLUMNS} {FROM} {JOIN} {WHERE} {ORDER}", result);
        }
    }
}
