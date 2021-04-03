using FS.Query.Scripts.Parameters;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Query.Scripts.Filters.Comparables
{
    public class ComparableEnumerable : SqlParameter
    {
        private readonly IEnumerable<object> values;
        private long? count;

        public ComparableEnumerable(ScriptParameters scriptParameters, bool isConstant, IEnumerable<object> values) : base(scriptParameters, isConstant)
        {
            this.values = values;
        }

        public long ParameterCount => count ??= values.Count(); 

        public override object BuildAsString(DbSettings dbSettings)
        {
            if (values is null || !values.Any())
                throw new ArgumentException("The informed enumerable can't be null.");

            var builder = new StringBuilder()
                .Append('(');

            bool addComma = false;
            foreach (var value in values)
            {
                if (addComma)
                    builder.Append(", ");
                else
                {
                    addComma = true;
                }

                if (value is null)
                {
                    builder.Append("NULL");
                    continue;
                }

                var conversion = SqlConversion.ToSql(value.GetType(), value);
                builder.Append(conversion);
            }

            builder.Append(')');

            return builder;
        }
    }
}
