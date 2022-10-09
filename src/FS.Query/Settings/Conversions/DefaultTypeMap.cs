using System;
using System.Collections.Generic;
using System.Data;

namespace FS.Query.Settings.Conversions
{
    public static class DefaultTypeMap
    {
        public static TypeMap GetDefaultMap(Type type) =>
            new(type, DbType.AnsiString, "VARCHAR", ToDatabaseConversion.Default, FromDatabaseConversion.Default);

        public static TypeMap GetDefaultEnumMap(Type type) =>
            new(type, DbType.AnsiString, "VARCHAR", ToDatabaseConversion.FromEnum, FromDatabaseConversion.ToEnum);

        public static Dictionary<Type, TypeMap> GetMap() => new(37)
        {
            [typeof(byte)] = new(
                typeof(byte),
                DbType.Byte,
                "TINYINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(sbyte)] = new TypeMap(
                typeof(sbyte),
                DbType.SByte,
                "TINYINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(short)] = new TypeMap(
                typeof(short),
                DbType.Int16,
                "SMALLINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(ushort)] = new TypeMap(
                typeof(ushort),
                DbType.UInt16,
                "SMALLINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(int)] = new TypeMap(
                typeof(int),
                DbType.Int32,
                "INT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(uint)] = new TypeMap(
                typeof(uint),
                DbType.UInt32,
                "INT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(long)] = new TypeMap(
                typeof(long),
                DbType.Int64,
                "BIGINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(ulong)] = new TypeMap(
                typeof(ulong),
                DbType.UInt64,
                "BIGINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(float)] = new TypeMap(
                typeof(float),
                DbType.Single,
                "FLOAT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(double)] = new TypeMap(
                typeof(double),
                DbType.Double,
                "DOUBLE",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(decimal)] = new TypeMap(
                typeof(decimal),
                DbType.Decimal,
                "DECIMAL",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(bool)] = new TypeMap(
                typeof(bool),
                DbType.Boolean,
                "BIT",
                ToDatabaseConversion.FromBool,
                FromDatabaseConversion.Default),

            [typeof(string)] = new TypeMap(
                typeof(string),
                DbType.String,
                "VARCHAR",
                ToDatabaseConversion.Default,
                FromDatabaseConversion.Default),

            [typeof(char)] = new TypeMap(
                typeof(char),
                DbType.StringFixedLength,
                "VARCHAR",
                ToDatabaseConversion.Default,
                FromDatabaseConversion.Default),

            [typeof(Guid)] = new TypeMap(
                typeof(Guid),
                DbType.Guid,
                "UNIQUEIDENTIFIER",
                ToDatabaseConversion.Default,
                FromDatabaseConversion.Default),

            [typeof(DateTime)] = new TypeMap(
                typeof(DateTime),
                DbType.DateTime,
                "DATETIME2",
                ToDatabaseConversion.FromDateTime,
                FromDatabaseConversion.Default),

            [typeof(DateTimeOffset)] = new TypeMap(
                typeof(DateTimeOffset),
                DbType.DateTimeOffset,
                "DATETIMEOFFSET",
                ToDatabaseConversion.FromDateTimeOff,
                FromDatabaseConversion.Default),

            [typeof(TimeSpan)] = new TypeMap(
                typeof(TimeSpan),
                DbType.Time,
                "BIGINT",
                ToDatabaseConversion.FromTimeSpan,
                FromDatabaseConversion.Default),

            [typeof(byte[])] = new TypeMap(
                typeof(byte[]),
                DbType.Binary,
                "VARBINARY",
                ToDatabaseConversion.Default,
                FromDatabaseConversion.Default),

            [typeof(byte?)] = new TypeMap(
                typeof(byte?),
                DbType.Byte,
                "TINYINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(sbyte?)] = new TypeMap(
                typeof(sbyte?),
                DbType.SByte,
                "TINYINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(short?)] = new TypeMap(
                typeof(short?),
                DbType.Int16,
                "SMALLINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(ushort?)] = new TypeMap(
                typeof(ushort?),
                DbType.UInt16,
                "SMALLINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(int?)] = new TypeMap(
                typeof(int?),
                DbType.Int32,
                "INT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(uint?)] = new TypeMap(
                typeof(uint?),
                DbType.UInt32,
                "INT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(long?)] = new TypeMap(
                typeof(long?),
                DbType.Int64,
                "BIGINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(ulong?)] = new TypeMap(
                typeof(ulong?),
                DbType.UInt64,
                "BIGINT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(float?)] = new TypeMap(
                typeof(float?),
                DbType.Single,
                "FLOAT",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(double?)] = new TypeMap(
                typeof(double?),
                DbType.Double,
                "DOUBLE",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(decimal?)] = new TypeMap(
                typeof(decimal?),
                DbType.Decimal,
                "DECIMAL",
                ToDatabaseConversion.FromNumber,
                FromDatabaseConversion.Default),

            [typeof(bool?)] = new TypeMap(
                typeof(bool?),
                DbType.Boolean,
                "BIT",
                ToDatabaseConversion.FromBool,
                FromDatabaseConversion.Default),

            [typeof(char?)] = new TypeMap(
                typeof(char?),
                DbType.StringFixedLength,
                "VARCHAR",
                ToDatabaseConversion.Default,
                FromDatabaseConversion.Default),

            [typeof(Guid?)] = new TypeMap(
                typeof(Guid?),
                DbType.Guid,
                "VARCHAR",
                ToDatabaseConversion.Default,
                FromDatabaseConversion.Default),

            [typeof(DateTime?)] = new TypeMap(
                typeof(DateTime?),
                DbType.DateTime,
                "DATETIME2",
                ToDatabaseConversion.FromDateTime,
                FromDatabaseConversion.Default),

            [typeof(DateTimeOffset?)] = new TypeMap(
                typeof(DateTimeOffset?),
                DbType.DateTimeOffset,
                "DATETIMEOFFSET",
                ToDatabaseConversion.FromDateTimeOff,
                FromDatabaseConversion.Default),

            [typeof(TimeSpan?)] = new TypeMap(
                typeof(TimeSpan?),
                DbType.Time,
                "BIGINT",
                ToDatabaseConversion.FromTimeSpan,
                FromDatabaseConversion.Default),

            [typeof(object)] = new TypeMap(
                typeof(object),
                DbType.Object,
                "VARCHAR",
                ToDatabaseConversion.Default,
                FromDatabaseConversion.Default),
        };
    }
}
