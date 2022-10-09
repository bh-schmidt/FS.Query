using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Combinations;
using FS.Query.Scripts.SelectionScripts.Combinations.Joins;
using FS.Query.Scripts.SelectionScripts.Sources;
using FS.Query.Settings;
using FS.Query.Settings.Builders;
using FS.Query.Tests.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FS.Query.Tests.Settings.Builders
{
    public class JoinBuilderTests
    {
        Mock<AliasTable> userTable = null!;
        Mock<AliasTable> postTable = null!;
        Mock<SelectionScript> selectionScript = null!;
        Mock<ExpressionJoin> expressionJoin = null!;

        [SetUp]
        public void Setup()
        {
            userTable = new Mock<AliasTable>(typeof(User), "user");
            postTable = new Mock<AliasTable>(typeof(UserPost), "post");
            selectionScript = new Mock<SelectionScript>(userTable.Object);
        }

        [Test]
        public void Will_build_the_join()
        {
            Expression<Func<User, UserPost, bool>> expression = (user, post) => user.Id == post.UserId;
            expressionJoin = new Mock<ExpressionJoin>(userTable.Object, postTable.Object, expression.Parameters.ToArray(), expression.Body);

            var combinations = new LinkedList<Combination>();
            combinations.AddLast(expressionJoin.Object);

            expressionJoin.Setup(e => e.Build(It.IsAny<DbSettings>()))
                .Returns("Join value");

            selectionScript.SetupGet(e => e.Combinations)
                .Returns(combinations);

            var builder = new JoinBuilder();
            var result = builder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.NotNull(result);
            Assert.AreEqual(" Join value", result!.ToString());
            selectionScript.Verify(e => e.Combinations, Times.Exactly(2));
            expressionJoin.Verify(e => e.Build(It.IsAny<DbSettings>()), Times.Exactly(1));
        }

        [Test]
        public void Wont_build_the_join_because_there_is_no_join()
        {
            var combinations = new LinkedList<Combination>();

            selectionScript.SetupGet(e => e.Combinations)
                .Returns(combinations);

            var builder = new JoinBuilder();
            var result = builder.Build(It.IsAny<DbSettings>(), selectionScript.Object);

            Assert.Null(result);
            selectionScript.Verify(e => e.Combinations, Times.Exactly(1));
        }
    }
}
