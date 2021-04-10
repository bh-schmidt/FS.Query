using System.Collections.Generic;

namespace FS.Query.Scripts.SelectionScripts.Parameters
{
    public class ScriptParameters
    {
        private readonly Dictionary<string, SqlParameter> parameters = new();

        public string Add(SqlParameter sqlParameter)
        {
            var key = $"@parameter{parameters.Count}";
            parameters.Add(key, sqlParameter);
            return key;
        }

        public SqlParameter? Get(string key) => parameters.GetValueOrDefault(key);
        public IEnumerable<KeyValuePair<string, SqlParameter>> Parameters => parameters;
    }
}
