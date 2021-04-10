using System;
using System.Data;

namespace FS.Query.Settings.Conversions
{
    public class TypeMap
    {
        public TypeMap(
            Type type,
            DbType dbType,
            string databaseType,
            Func<object, object> toDatabaseConversion)
        {
            Type = type;
            DbType = dbType;
            DatabaseType = databaseType;
            ToDatabaseConversion = toDatabaseConversion;
        }

        public Type Type { get; set; }
        public DbType DbType { get; set; }
        public string DatabaseType { get; set; }
        public Func<object, object> ToDatabaseConversion { get; }
    }
}
