using FS.Query.Scripts.Combinations;
using FS.Query.Scripts.Parameters;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Data;

namespace FS.Query.Scripts.Filters.Comparables
{
    public class ComparableValue : SqlParameter
    {
        private readonly object value;

        public ComparableValue(object value, ScriptParameters scriptParameters, bool isConstant) : base(scriptParameters, isConstant)
        {
            this.value = value;
            Type = value.GetObjectType();
        }

        public DbType? DbType { get; set; }
        public Type Type { get; }

        public override object BuildAsString(DbSettings dbSettings) => SqlConversion.ToSql(Type, value);

        public void AddParameter(string parameterName, IDbDataParameter dbDataParameter, DbSettings dbSettings)
        {
            dbDataParameter.ParameterName = parameterName;
            dbDataParameter.Value = value;
            dbDataParameter.DbType = DbType ??= dbSettings.TypeMap.GetDbType(Type);
        }

        public override bool Equals(object? obj) => obj is ComparableValue value && EqualityComparer<Type>.Default.Equals(Type, value.Type);
        public override int GetHashCode() => HashCode.Combine(Type);
    }
}
