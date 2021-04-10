using FS.Query.Scripts.SelectionScripts.Selects;
using FS.Query.Scripts.SelectionScripts.Serializations;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FS.Query.Scripts.SelectionScripts.Runners
{
    public class ScriptRunner<T>
    {
        private readonly BuildedScript buildedScript;
        private readonly DbManager databaseManager;

        public ScriptRunner(BuildedScript buildedScript, DbManager databaseManager)
        {
            this.buildedScript = buildedScript;
            this.databaseManager = databaseManager;
        }

        public T[] GetArray()
        {

            var connection = databaseManager.Connection;
            var command = buildedScript.PrepareCommand(databaseManager.DbSettings, buildedScript.ScriptParameters, connection);

            var reader = command.ExecuteReader();
            var result = Read(reader, databaseManager.DbSettings, buildedScript.SelectColumns);

            return result.ToArray();
        }

        private static IEnumerable<T> Read(IDataReader reader, DbSettings dbSettings, Select[] selectColumns)
        {
            var scriptSerialization = new ScriptSerialization(typeof(T), selectColumns, reader, dbSettings);

            var serializationType = typeof(T);

            while (reader.Read())
            {
                var obj = (T)Activator.CreateInstance(serializationType)!;
                scriptSerialization.Serialize(reader, obj);
                yield return obj;
            }
        }
    }
}
