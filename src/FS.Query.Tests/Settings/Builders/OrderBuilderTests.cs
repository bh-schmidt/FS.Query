using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Orders;
using FS.Query.Settings;
using FS.Query.Settings.Builders;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace FS.Query.Tests.Settings.Builders
{
    public class OrderBuilderTests
    {
        readonly OrderBuilder orderBuilder = new();
        Mock<SelectionScript> selectionScript = null!;
        Mock<ColumnOrder> columnOrder = null!;

        [SetUp]
        public void Setup()
        {
            selectionScript = new(null);
            columnOrder = new(null, false);
        }

        [Test]
        public void Will_build_the_order()
        {
            var orders = new LinkedList<ColumnOrder>();
            orders.AddLast(columnOrder.Object);

            columnOrder.Setup(e => e.ScriptColumn.BuildWithAlias(It.IsAny<DbSettings>()))
                .Returns("ID");

            selectionScript.Setup(e => e.Orders)
                .Returns(orders);

            var result = orderBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.NotNull(result);
            Assert.AreEqual(" ORDER BY ID", result!.ToString());
            columnOrder.Verify(e => e.ScriptColumn.BuildWithAlias(It.IsAny<DbSettings>()), Times.Once);
            selectionScript.Verify(e => e.Orders, Times.Exactly(2));
        }

        [Test]
        public void Will_build_the_order_with_two_columns()
        {
            var orders = new LinkedList<ColumnOrder>();
            orders.AddLast(columnOrder.Object);
            orders.AddLast(columnOrder.Object);

            columnOrder.SetupSequence(e => e.Descending)
                .Returns(false)
                .Returns(true);

            columnOrder.SetupSequence(e => e.ScriptColumn.BuildWithAlias(It.IsAny<DbSettings>()))
                .Returns("ID")
                .Returns("NAME");

            selectionScript.Setup(e => e.Orders)
                .Returns(orders);

            var result = orderBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.NotNull(result);
            Assert.AreEqual(" ORDER BY ID, NAME DESC", result!.ToString());
            columnOrder.Verify(e => e.ScriptColumn.BuildWithAlias(It.IsAny<DbSettings>()), Times.Exactly(2));
            selectionScript.Verify(e => e.Orders, Times.Exactly(2));
        }


        [Test]
        public void Wont_build_because_there_is_no_column_order()
        {
            var orders = new LinkedList<ColumnOrder>();

            selectionScript.Setup(e => e.Orders)
                .Returns(orders);

            var result = orderBuilder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.Null(result);
            selectionScript.Verify(e => e.Orders, Times.Once);
        }
    }
}
