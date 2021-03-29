using FS.Query.Scripts.Filters;
using System;
using System.Text;

namespace FS.Query.Settings.Mapping
{
    public class PropertyMap 
    {
        private string columnName = "";

        public PropertyMap(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            PropertyName = name;
            ColumnName = name;
        }

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
    }
}
