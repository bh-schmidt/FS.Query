using System;
using System.Data;

namespace FS.Query.Settings.Conversions
{
    public class TypeMap
    {
        private readonly Func<object, object> toDatabaseConversion; 
        
        public TypeMap(
            Type type,
            DbType dbType,
            string databaseType,
            Func<object, object> toDatabaseConversion,
            Func<Type, object, object> fromDatabaseConversion)
        {
            Type = type;
            DbType = dbType;
            DatabaseType = databaseType;
            this.toDatabaseConversion = toDatabaseConversion;
            FromDatabaseConversion = fromDatabaseConversion;
        }

        public virtual Type Type { get; set; }
        public virtual DbType DbType { get; set; }
        public virtual string DatabaseType { get; set; }
        public virtual Func<Type, object, object> FromDatabaseConversion { get; }

        public virtual object ToDatabaseConversion(object? obj)
        {
            if (obj is null)
                return "NULL";

            return toDatabaseConversion(obj);
        }
    }
}
