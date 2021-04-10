using FS.Query.Scripts.SelectionScripts;
using System;
using System.Collections.Generic;
using System.Data;

namespace FS.Query.Settings.Caching
{
    public static class DefaultTypeMap
    {
        public static TypeMap GetDefaultMap(Type type) =>
            new TypeMap(type, DbType.AnsiString, "VARCHAR", ToDatabaseConversion.DefaultToDatabaseConversion);

        public static Dictionary<Type, TypeMap> GetMap() => new(37)
        {
            [typeof(byte)] = new TypeMap(
                typeof(byte),
                DbType.Byte,
                "TINYINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(sbyte)] = new TypeMap(
                typeof(sbyte),
                DbType.SByte,
                "TINYINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(short)] = new TypeMap(
                typeof(short),
                DbType.Int16,
                "SMALLINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(ushort)] = new TypeMap(
                typeof(ushort),
                DbType.UInt16,
                "SMALLINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(int)] = new TypeMap(
                typeof(int),
                DbType.Int32,
                "INT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(uint)] = new TypeMap(
                typeof(uint),
                DbType.UInt32,
                "INT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(long)] = new TypeMap(
                typeof(long),
                DbType.Int64,
                "BIGINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(ulong)] = new TypeMap(
                typeof(ulong),
                DbType.UInt64,
                "BIGINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(float)] = new TypeMap(
                typeof(float),
                DbType.Single,
                "FLOAT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(double)] = new TypeMap(
                typeof(double),
                DbType.Double,
                "DOUBLE",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(decimal)] = new TypeMap(
                typeof(decimal),
                DbType.Decimal,
                "DECIMAL",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(bool)] = new TypeMap(
                typeof(bool),
                DbType.Boolean,
                "BIT",
                ToDatabaseConversion.BoolToDatabaseConversion),

            [typeof(string)] = new TypeMap(
                typeof(string),
                DbType.String,
                "VARCHAR",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(char)] = new TypeMap(
                typeof(char),
                DbType.StringFixedLength,
                "VARCHAR",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(Guid)] = new TypeMap(
                typeof(Guid),
                DbType.Guid,
                "UNIQUEIDENTIFIER",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(DateTime)] = new TypeMap(
                typeof(DateTime),
                DbType.DateTime,
                "DATETIME2",
                ToDatabaseConversion.DateTimeToDatabaseConversion),

            [typeof(DateTimeOffset)] = new TypeMap(
                typeof(DateTimeOffset),
                DbType.DateTimeOffset,
                "DATETIMEOFFSET",
                ToDatabaseConversion.DateTimeOffsetToDatabaseConversion),

            [typeof(TimeSpan)] = new TypeMap(
                typeof(TimeSpan),
                DbType.Time,
                "BIGINT",
                ToDatabaseConversion.TimeSpanToDatabaseConversion),

            [typeof(byte[])] = new TypeMap(
                typeof(byte[]),
                DbType.Binary,
                "VARBINARY",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(byte?)] = new TypeMap(
                typeof(byte?),
                DbType.Byte,
                "TINYINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(sbyte?)] = new TypeMap(
                typeof(sbyte?),
                DbType.SByte,
                "TINYINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(short?)] = new TypeMap(
                typeof(short?),
                DbType.Int16,
                "SMALLINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(ushort?)] = new TypeMap(
                typeof(ushort?),
                DbType.UInt16,
                "SMALLINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(int?)] = new TypeMap(
                typeof(int?),
                DbType.Int32,
                "INT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(uint?)] = new TypeMap(
                typeof(uint?),
                DbType.UInt32,
                "INT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(long?)] = new TypeMap(
                typeof(long?),
                DbType.Int64,
                "BIGINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(ulong?)] = new TypeMap(
                typeof(ulong?),
                DbType.UInt64,
                "BIGINT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(float?)] = new TypeMap(
                typeof(float?),
                DbType.Single,
                "FLOAT",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(double?)] = new TypeMap(
                typeof(double?),
                DbType.Double,
                "DOUBLE",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(decimal?)] = new TypeMap(
                typeof(decimal?),
                DbType.Decimal,
                "DECIMAL",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(bool?)] = new TypeMap(
                typeof(bool?),
                DbType.Boolean,
                "BIT",
                ToDatabaseConversion.BoolToDatabaseConversion),

            [typeof(char?)] = new TypeMap(
                typeof(char?),
                DbType.StringFixedLength,
                "VARCHAR",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(Guid?)] = new TypeMap(
                typeof(Guid?),
                DbType.Guid,
                "VARCHAR",
                ToDatabaseConversion.DefaultToDatabaseConversion),

            [typeof(DateTime?)] = new TypeMap(
                typeof(DateTime?),
                DbType.DateTime,
                "DATETIME2",
                ToDatabaseConversion.DateTimeToDatabaseConversion),

            [typeof(DateTimeOffset?)] = new TypeMap(
                typeof(DateTimeOffset?),
                DbType.DateTimeOffset,
                "DATETIMEOFFSET",
                ToDatabaseConversion.DateTimeOffsetToDatabaseConversion),

            [typeof(TimeSpan?)] = new TypeMap(
                typeof(TimeSpan?),
                DbType.Time,
                "BIGINT",
                ToDatabaseConversion.TimeSpanToDatabaseConversion),

            [typeof(object)] = new TypeMap(
                typeof(object),
                DbType.Object,
                "VARCHAR",
                ToDatabaseConversion.DefaultToDatabaseConversion),
        };
    }
}
