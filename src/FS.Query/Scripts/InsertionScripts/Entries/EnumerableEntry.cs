using FS.Query.Scripts.Columns;
using FS.Query.Settings;
using FS.Query.Settings.Conversions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FS.Query.Scripts.InsertionScripts.Entries
{
    public class EnumerableEntry : Entry
    {
        private const string comma = ", ";
        private const char OpenParenthesis = '(';
        private const char CloseParenthesis = ')';
        readonly List<PropertyInfo> propertyInfos;
        readonly List<TypeMap> typeMaps;

        public EnumerableEntry(
            Type type,
            IEnumerable<object> values,
            DbSettings dbSettings,
            IEnumerable<IColumn> scriptColumns) : base(scriptColumns)
        {
            if (type is null)
                throw new ArgumentException("The type can't be null.");

            Type = type;
            Values = values;

            var totalColums = scriptColumns.Count();
            propertyInfos = new(totalColums);
            typeMaps = new(totalColums);
            SetPropertyInfos(dbSettings, ScriptColumns, propertyInfos, typeMaps);
        }

        public virtual Type Type { get; }
        public virtual IEnumerable<object> Values { get; }

        public override object Build(DbSettings dbSettings)
        {
            if (Values is null || !Values.Any())
                throw new ArgumentException("The values can't be null or empty.");

            var valuesArray = Values.ToArray();
            var lastValueIndex = valuesArray.Length - 1;
            var lastPropertyIndex = propertyInfos.Count - 1;
            StringBuilder stringBuilder = new();

            for (int valueIndex = 0; valueIndex < valuesArray.Length; valueIndex++)
            {
                var value = valuesArray[valueIndex];
                stringBuilder.Append(OpenParenthesis);
                for (int propertyIndex = 0; propertyIndex < propertyInfos.Count; propertyIndex++)
                {
                    var propertyInfo = propertyInfos[propertyIndex];
                    var typeMap = typeMaps[propertyIndex];
                    var propertyValue = propertyInfo.GetValue(value);
                    var treatedProperty = typeMap.ToDatabaseConversion(propertyValue);

                    stringBuilder.Append(treatedProperty);

                    if (propertyIndex != lastPropertyIndex)
                        stringBuilder.Append(comma);
                }
                stringBuilder.Append(CloseParenthesis);

                if (valueIndex != lastValueIndex)
                    stringBuilder.Append(comma);
            }

            return stringBuilder;
        }

        private void SetPropertyInfos(
            DbSettings dbSettings,
            IEnumerable<IColumn> scriptColumns,
            List<PropertyInfo> propertyInfos,
            List<TypeMap> typeMaps)
        {
            (PropertyInfo, TypeMap) property;

            foreach (var column in scriptColumns)
            {
                if (column is AliasTableColumn tableProperty)
                {
                    property = GetPropertyInfo(tableProperty.PropertyName, dbSettings);
                    propertyInfos.Add(property.Item1);
                    typeMaps.Add(property.Item2);
                    continue;
                }

                column.Build(dbSettings);
                property = GetPropertyInfo(column.ColumnName, dbSettings);
                propertyInfos.Add(property.Item1);
                typeMaps.Add(property.Item2);
            }
        }

        private (PropertyInfo, TypeMap) GetPropertyInfo(string property, DbSettings dbSettings)
        {
            var propertyInfo = Type.GetProperty(property);
            if (propertyInfo is null)
                throw new ArgumentException("The column/property informed does not exist in the object type.");

            var typeMap = dbSettings.TypeMapping.GetTypeMap(propertyInfo.PropertyType);
            return (propertyInfo, typeMap);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not EnumerableEntry entry || entry.Type != Type)
                return false;

            return entry.ScriptColumns.SequenceEqual(ScriptColumns);
        }

        public override int GetHashCode()
        {
            var hash = Type.GetHashCode();
            foreach (var column in ScriptColumns) hash = HashCode.Combine(hash, column);
            return hash;
        }
    }
}
