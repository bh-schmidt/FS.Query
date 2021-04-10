using FS.Query.Builders;
using FS.Query.Settings;
using FS.Query.Tests.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Data;

namespace FS.Query.Tests
{
    public class DbManagerTests
    {
        Mock<DbSettings> dbSettings = null!;
        Mock<IServiceProvider> serviceProvider = null!;
        Mock<IDbConnection> dbConnection = null!;
        Mock<Connection> connection = null!;
        Mock<Func<IServiceProvider, IDbConnection>> createConnectionMock;
        DbManager dbManager = null!;

        [SetUp]
        public void Setup()
        {
            dbSettings = new();
            serviceProvider = new();
            dbConnection = new();
            createConnectionMock = new();
            connection = new(null);
            dbManager = new(dbSettings.Object, serviceProvider.Object);
        }

        [Test]
        public void Will_create_a_new_connection_one_time()
        {
            createConnectionMock
                .Setup(e => e(It.IsAny<IServiceProvider>()))
                .Returns(dbConnection.Object);

            connection
                .Setup(e => e.CreateConnection)
                .Returns(createConnectionMock.Object);

            dbSettings
                .SetupGet(e => e.Connection)
                .Returns(() => connection.Object);

            _ = dbManager.Connection;
            _ = dbManager.Connection;

            createConnectionMock.Verify(e => e.Invoke(It.IsAny<IServiceProvider>()), Times.Once);
        }

        [Test]
        public void Will_create_a_new_unique_connection()
        {
            createConnectionMock
                .Setup(e => e(It.IsAny<IServiceProvider>()))
                .Returns(dbConnection.Object);

            connection
                .Setup(e => e.CreateConnection)
                .Returns(createConnectionMock.Object);

            dbSettings
                .SetupGet(e => e.Connection)
                .Returns(() => connection.Object);

            _ = dbManager.UniqueConnection;
            _ = dbManager.UniqueConnection;

            createConnectionMock.Verify(e => e.Invoke(It.IsAny<IServiceProvider>()), Times.Exactly(2));
        }

        [Test]
        public void Will_dispose_the_connections()
        {
            connection
                .Setup(e => e.CreateConnection)
                .Returns(e => dbConnection.Object);

            dbSettings
                .SetupGet(e => e.Connection)
                .Returns(() => connection.Object);

            _ = dbManager.Connection;
            _ = dbManager.UniqueConnection;
            _ = dbManager.UniqueConnection;

            dbManager.Dispose();

            dbConnection.Verify(e => e.Dispose(), Times.Exactly(3));
        }

        [Test]
        public void Will_throw_function_to_create_connection_not_set()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = dbManager.Connection;
            }, "The function to create the connection wasn't set.");

            Assert.Throws<ArgumentException>(() =>
            {
                _ = dbManager.UniqueConnection;
            }, "The function to create the connection wasn't set.");
        }

        [Test]
        public void Will_create_a_table_builder()
        {
            var result = dbManager.FromTable<User>("p");
            Assert.IsInstanceOf<SelectionByTableBuilder<User>>(result);
        }

        [Test]
        public void Will_create_a_script_injection_builder()
        {
            var result = dbManager.FromScript("p", "SELECT * FROM [USER]");
            Assert.IsInstanceOf<SelectionBuilder>(result);
        }
    }
}
