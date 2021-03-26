using System;
using System.Data;

namespace FS.Query.Mapping
{
    public class PropertyMap
    {
        private string columnName = "";
        private Action<object, IDataReader, int> FillValue;

        public PropertyMap(Type type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            Type = type;
            PropertyName = name;
            ColumnName = name;
            FillValue = GetValueConversion(type);
        }

        public Type Type { get; }
        public string PropertyName { get; set; }
        public string ColumnName
        {
            get => columnName;
            set
            {
                columnName = value ?? PropertyName;
                TreatedColumnName = $"[{columnName}]";
            }
        }
        public string TreatedColumnName { get; private set; } = "";

        public void Fill(object obj, IDataReader dataReader, int propertyIndex)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (dataReader is null)
                throw new ArgumentNullException(nameof(dataReader));

            FillValue(obj, dataReader, propertyIndex);
        }

        private Action<object, IDataReader, int> GetValueConversion(Type type)
        {
            Action<object, IDataReader, int> action;

            action = SetDefault;

            return action;
        }

        private void SetDefault(object obj, IDataReader dataReader, int propertyIndex)
        {
            var value = dataReader.GetValue(propertyIndex);
            var type = obj.GetType();
            var propertyInfo = type.GetProperty(ColumnName);

            if (propertyInfo is null)
                return;

            propertyInfo.SetValue(obj, value);
        }
    }
}
