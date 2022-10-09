using FS.Query.Settings;
using FS.Query.Settings.Conversions;
using FS.Query.Settings.Mapping;
using FS.Query.Tests.Shared;
using NUnit.Framework;
using System;
using System.Data;

namespace FS.Query.Tests.Settings.Mapping
{
    public class PropertyMapTests
    {
        [Test]
        public void Will_create_a_property_map()
        {
            var map = new PropertyMap("name", typeof(string));

            Assert.AreEqual("name", map.PropertyName);
            Assert.AreEqual("name", map.ColumnName);
            Assert.AreEqual("[name]", map.TreatedColumnName);
            Assert.AreEqual(typeof(string), map.PropertyType);
        }

        [Test]
        public void Will_throw_empty_name()
        {
            Assert.Throws<ArgumentException>(() => new PropertyMap("", typeof(string)));
        }

        [Test]
        public void Will_build_the_propery()
        {
            var dbSettings = new DbSettings
            {
                TypeMapping = new TypeMapping()
            };
            var map = new PropertyMap("name", typeof(string));
            map.Build(dbSettings);

            Assert.AreEqual(DbType.String, map.DbType);
        }
    }
}
