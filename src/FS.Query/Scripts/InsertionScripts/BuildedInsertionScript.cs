using FS.Query.Scripts.Columns;
using FS.Query.Scripts.InsertionScripts.Entries;
using FS.Query.Settings;
using System;
using System.Text;

namespace FS.Query.Scripts.InsertionScripts
{
    public class BuildedInsertionScript
    {
        public BuildedInsertionScript(
            string buildedScript,
            IColumn[] columns)
        {
            if (string.IsNullOrEmpty(buildedScript))
                throw new ArgumentException($"The buildedScript cannot be null or empty.", nameof(buildedScript));

            if (columns is null || columns.Length == 0)
                throw new ArgumentException("The columns can't be null or empty.");

            BuildedScript = buildedScript;
            Columns = columns;
        }

        public string BuildedScript { get; }
        public IColumn[] Columns { get; }

        public string Build(DbSettings dbSettings, Entry entry)
        {
            if (entry is null)
                throw new ArgumentException("Entry can't be null");

            var buildedEntry = entry.Build(dbSettings);

            if (buildedEntry is null)
                throw new Exception("The builded entry can't be null.");

            var stringBuilder = new StringBuilder(BuildedScript)
                .Replace("@entry", buildedEntry.ToString());

            return stringBuilder.ToString();
        }
    }
}
