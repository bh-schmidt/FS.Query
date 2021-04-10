using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Filters;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Query.Scripts.InsertionScripts.Entries
{
    public class EnumerableEntry : Entry
    {
        private const string comma = ", ";
        private const char OpenParenthesis = '(';
        private const char CloseParenthesis = ')';

        public EnumerableEntry(Type type, IEnumerable<object> values)
        {
            Type = type;
            Values = values;
        }

        public virtual Type Type { get; }
        public virtual IEnumerable<object> Values { get; }

        public override object Build(DbSettings dbSettings, IEnumerable<IScriptColumn> scriptColumns)
        {
            if (Type is null)
                throw new ArgumentException("The type can't be null.");

            if (Values is null || !Values.Any())
                throw new ArgumentException("The values can't be null or empty.");

            if (scriptColumns is null || !scriptColumns.Any())
                throw new ArgumentException("The script columns can't be null or empty.");

            List<string> properties = GetProperties(dbSettings, scriptColumns);

            var valuesArray = Values.ToArray();
            var lastValueIndex = valuesArray.Length - 1;
            var lastPropertyIndex = properties.Count - 1;
            StringBuilder stringBuilder = new();

            for (int valueIndex = 0; valueIndex < valuesArray.Length; valueIndex++)
            {
                stringBuilder.Append(OpenParenthesis);
                for (int propertyIndex = 0; propertyIndex < properties.Count; propertyIndex++)
                {
                    var propertyInfo = Type.GetProperty(properties[propertyIndex])!;
                    var treatedValue = dbSettings.TypeMapping.ToSql(propertyInfo.PropertyType, valuesArray[valueIndex]);

                    stringBuilder.Append(treatedValue);

                    if (propertyIndex != lastPropertyIndex)
                        stringBuilder.Append(comma);
                }
                stringBuilder.Append(CloseParenthesis);

                if (valueIndex != lastValueIndex)
                    stringBuilder.Append(comma);
            }

            return stringBuilder;
        }

        private List<string> GetProperties(DbSettings dbSettings, IEnumerable<IScriptColumn> scriptColumns)
        {
            List<string> properties = new(scriptColumns.Count());

            foreach (var column in scriptColumns)
            {
                if (column is TableProperty tableProperty)
                {
                    properties.Add(tableProperty.PropertyName);
                    ValidateProperty(tableProperty.PropertyName);
                    continue;
                }

                column.Build(dbSettings);
                properties.Add(column.ColumnName);
                ValidateProperty(column.ColumnName);
            }

            return properties;
        }

        private void ValidateProperty(string property)
        {
            var propertyInfo = Type.GetProperty(property);
            if (propertyInfo is null)
                throw new ArgumentException("The column/property informed does not exist in the object type.");
        }
    }
}
