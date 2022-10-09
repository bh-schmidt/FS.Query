using FS.Query.Settings.Conversions;
using FS.Query.Tests.Shared;
using NUnit.Framework;
using System;
using System.Data;

namespace FS.Query.Tests.Settings.Conversions
{
    public class TypeMappingTests
    {
        readonly TypeMapping typeMapping = new();

        [Test]
        public void Will_return_a_mapped_type_map()
        {
            var typeMap = typeMapping.GetTypeMap(typeof(Guid));
            Assert.NotNull(typeMap);
            Assert.AreEqual(typeof(Guid), typeMap.Type);
        }

        [Test]
        public void Will_return_an_enum_mapped_type_map()
        {
            var typeMap = typeMapping.GetTypeMap(typeof(DbType));
            Assert.NotNull(typeMap);
            Assert.AreEqual(typeof(DbType), typeMap.Type);
        }

        [Test]
        public void Will_return_the_default_type_map()
        {
            var typeMap = typeMapping.GetTypeMap(typeof(User));
            Assert.NotNull(typeMap);
            Assert.AreEqual(typeof(User), typeMap.Type);
        }

        [Test]
        public void Will_map_a_null_value_to_sql()
        {
            var result = typeMapping.MapToSql(typeof(string), null);
            Assert.NotNull(result);
            Assert.AreEqual("NULL", result);
        }

        [Test]
        public void Will_map_a_boolean_to_sql()
        {
            var result = typeMapping.MapToSql(typeof(bool), true);
            Assert.NotNull(result);
            Assert.AreEqual(1, result);
        }
    }
}
