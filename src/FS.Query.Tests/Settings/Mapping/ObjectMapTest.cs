using FS.Query.Settings;
using FS.Query.Settings.Mapping;
using FS.Query.Tests.Shared;
using Moq;
using NUnit.Framework;

namespace FS.Query.Tests.Settings.Mapping
{
    public class ObjectMapTest
    {
        [Test]
        public void Will_create_an_object_map()
        {
            var objectMap = new ObjectMap(typeof(User));

            Assert.AreEqual(typeof(User), objectMap.Type);
            Assert.AreEqual(nameof(User), objectMap.TableName);
            Assert.AreEqual("dbo", objectMap.TableSchema);
            Assert.AreEqual($"[dbo].[{nameof(User)}]", objectMap.TableFullName);
        }

        [Test]
        public void Will_change_the_table_name()
        {
            var objectMap = new ObjectMap(typeof(User))
            {
                TableName = "NewTableName"
            };

            Assert.AreEqual("NewTableName", objectMap.TableName);
            Assert.AreEqual("[dbo].[NewTableName]", objectMap.TableFullName);
        }

        [Test]
        public void Will_change_the_table_schema()
        {
            var objectMap = new ObjectMap(typeof(User))
            {
                TableSchema = "NewTableSchema"
            };

            Assert.AreEqual("NewTableSchema", objectMap.TableSchema);
            Assert.AreEqual($"[NewTableSchema].[{nameof(User)}]", objectMap.TableFullName);
        }

        [Test]
        public void Will_build_the_object_map()
        {
            var objectMap = new ObjectMap(typeof(User));
            var propertyMap = new Mock<PropertyMap>("Property", typeof(string));

            objectMap.PropertyMaps.Add(propertyMap.Object);
            objectMap.Build(It.IsAny<DbSettings>());

            propertyMap.Verify(e => e.Build(It.IsAny<DbSettings>()), Times.Once);
        }
    }
}
