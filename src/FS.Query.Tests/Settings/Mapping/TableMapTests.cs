using FS.Query.Settings.Mapping;
using FS.Query.Tests.Shared;
using NUnit.Framework;
using FS.Query.Builders.Mapping;

namespace FS.Query.Tests.Settings.Mapping
{
    public class TableMapTests
    {
        TableMap<User> tableMap = null!;

        [SetUp]
        public void Setup()
        {
            tableMap = new TableMap<User>();

        }

        [Test]
        public void Will_add_the_table_name()
        {
            tableMap.TableName("table name");

            Assert.AreEqual("table name", tableMap.ObjectMap.TableName);
        }

        [Test]
        public void Will_add_the_table_schema()
        {
            tableMap.TableSchema("table schema");

            Assert.AreEqual("table schema", tableMap.ObjectMap.TableSchema);
        }

        [Test]
        public void Will_add_a_new_property()
        {
            tableMap.Property(e => e.Id); 
            Assert.AreEqual(1, tableMap.ObjectMap.PropertyMaps.Count);
        }

        [Test]
        public void Will_ignore_the_second_property()
        {
            tableMap.Property(e => e.Id); 
            tableMap.Property(e => e.Id); 
            Assert.AreEqual(1, tableMap.ObjectMap.PropertyMaps.Count);
        }
    }
}
