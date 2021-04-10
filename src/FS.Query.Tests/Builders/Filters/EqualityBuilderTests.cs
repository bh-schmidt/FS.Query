using FS.Query.Builders.Filters;
using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Scripts.SelectionScripts.Filters.Comparables;
using FS.Query.Scripts.SelectionScripts.Operators;
using FS.Query.Settings;
using FS.Query.Tests.Shared;
using Moq;
using NUnit.Framework;

namespace FS.Query.Tests.Builders.Filters
{
    public class EqualityBuilderTests
    {
        EqualityBuilder equalityBuilder = null!;
        Mock<SelectionScript> selectionScript = null!;
        Mock<ComparationBlockBuilder> comparationBlockBuilder = null!;
        Mock<LogicalConnectiveBuilder> logicalConnectiveBuilder = null!;
        Mock<ComparationNode> comparationNode = null!;
        Mock<ISqlComparable> fistComparable = null!;

        [SetUp]
        public void Setup()
        {
            selectionScript = new(null);
            comparationBlockBuilder = new(null);
            logicalConnectiveBuilder = new(null);
            fistComparable = new();
            comparationNode = new(null, null, null);

            fistComparable.Setup(e => e.Build(It.IsAny<DbSettings>()));

            equalityBuilder = new(selectionScript.Object, comparationBlockBuilder.Object, logicalConnectiveBuilder.Object, fistComparable.Object);
        }

        [Test]
        public void Will_create_an_enumerable_filter_of_in()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.In(new[] { 1, 2, 3 });

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.In, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<ComparableEnumerable>(lastNode.Second);
        }

        [Test]
        public void Will_create_an_enumerable_filter_of_not_in()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.NotIn(new[] { 1, 2, 3 });

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.NotIn, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<ComparableEnumerable>(lastNode.Second);
        }

        [Test]
        public void Will_create_a_column_filter_of_equal()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.Equals<User>("u", e => e.Id);

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.Equal, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<TableProperty>(lastNode.Second);
        }

        [Test]
        public void Will_create_an_object_filter_of_equal()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.Equals("Id");

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.Equal, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<ComparableValue>(lastNode.Second);
        }

        [Test]
        public void Will_create_a_column_filter_of_not_equal()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.NotEqual<User>("u", e => e.Id);

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.NotEqual, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<TableProperty>(lastNode.Second);
        }

        [Test]
        public void Will_create_an_object_filter_of_not_equal()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.NotEqual("Id");

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.NotEqual, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<ComparableValue>(lastNode.Second);
        }

        [Test]
        public void Will_create_a_column_filter_of_not_greater()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.Greater<User>("u", e => e.Id);

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.GreaterThan, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<TableProperty>(lastNode.Second);
        }

        [Test]
        public void Will_create_an_object_filter_of_not_greater()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.Greater("Id");

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.GreaterThan, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<ComparableValue>(lastNode.Second);
        }

        [Test]
        public void Will_create_a_column_filter_of_not_greater_or_equal()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.GreaterOrEqual<User>("u", e => e.Id);

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.GreaterThanOrEqual, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<TableProperty>(lastNode.Second);
        }

        [Test]
        public void Will_create_an_object_filter_of_not_greater_or_equal()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.GreaterOrEqual("Id");

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.GreaterThanOrEqual, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<ComparableValue>(lastNode.Second);
        }

        [Test]
        public void Will_create_a_column_filter_of_not_less()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.Less<User>("u", e => e.Id);

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.LessThan, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<TableProperty>(lastNode.Second);
        }

        [Test]
        public void Will_create_an_object_filter_of_not_less()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.Less("Id");

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.LessThan, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<ComparableValue>(lastNode.Second);
        }

        [Test]
        public void Will_create_a_column_filter_of_not_less_or_equal()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.LessOrEqual<User>("u", e => e.Id);

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.LessThanOrEqual, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<TableProperty>(lastNode.Second);
        }

        [Test]
        public void Will_create_an_object_filter_of_not_less_or_equal()
        {
            ComparationNode lastNode = null!;
            comparationBlockBuilder
                .SetupSet(e => e.LastNode = It.IsAny<ComparationNode>())
                .Callback<ComparationNode>(e => lastNode = e);

            equalityBuilder.LessOrEqual("Id");

            Assert.NotNull(lastNode);
            Assert.AreEqual(fistComparable.Object, lastNode.First);
            Assert.AreEqual(Operator.LessThanOrEqual, lastNode.Operator);
            Assert.NotNull(lastNode.Second);
            Assert.IsInstanceOf<ComparableValue>(lastNode.Second);
        }

        [Test]
        public void Will_add_two_comparation_node()
        {
            comparationBlockBuilder.Setup(e => e.LastNode)
                .Returns(comparationNode.Object);

            equalityBuilder.Equals("Id");

            comparationBlockBuilder.Verify(e => e.LastNode, Times.Exactly(2));
            comparationNode.VerifySet(e => e.NextNode = It.IsAny<ComparationNode>(), Times.Once);
        }
    }
}
