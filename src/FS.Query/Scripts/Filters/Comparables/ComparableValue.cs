using FS.Query.Scripts.Combinations;
using FS.Query.Scripts.Parameters;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FS.Query.Scripts.Filters.Comparables
{
    public class ComparableValue : SqlParameter
    {
        private readonly Type type;
        private readonly object value;

        public ComparableValue(object value, ScriptParameters scriptParameters, bool isConstant) : base(scriptParameters, isConstant)
        {
            this.value = value;
            type = value.GetObjectType();
        }

        public override object BuildParameter(DbSettings dbSettings) => SqlConversion.ToSql(type, value);

        public override bool Equals(object? obj) => obj is ComparableValue value && EqualityComparer<Type>.Default.Equals(type, value.type);
        public override int GetHashCode() => HashCode.Combine(type);
    }
}
