using FS.Query.Builders;
using FS.Query.Settings;
using FS.Query.Settings.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FS.Query.Scripts.Runners
{
    public static class ScriptRunner
    {
        public static IEnumerable<T> Run<T>(Script script, DbManager databaseManager)
            where T : new()
        {
            var buildedScript = databaseManager
                .DbSettings
                .ScriptCache
                .GetOrCreate(script, databaseManager.DbSettings);

            var apply = buildedScript.GetScript(databaseManager.DbSettings, script.ScriptParameters);

            var connection = databaseManager.GetConnection();
            var command = connection.CreateCommand();

            command.CommandText = apply;
            command.CommandTimeout = 30;
            command.CommandType = CommandType.Text;

            var reader = command.ExecuteReader();
            var result = Read<T>(reader, databaseManager.DbSettings);

            return result.ToArray();
        }

        private static IEnumerable<T> Read<T>(IDataReader reader, DbSettings dbSettings)
            where T : new()
        {
            var propertyMaps = GetPropertyMaps(reader, dbSettings, typeof(T)).ToArray();
            //var properties = map.PropertyMaps.Where();


            while (reader.Read())
            {
                var obj = new T();
                for (int i = 0; i < propertyMaps.Length; i++)
                {
                    var tuple = propertyMaps[i];
                    var propertyIndex = tuple.Item1;
                    var propertyMap = tuple.Item2;
                    
                    //propertyMap.
                    //var propertyMap = script.GetProperty(index);
                    //propertyMap.Fill(obj, reader, index);
                }
                yield return obj;
            }
        }

        private static IEnumerable<(int, PropertyMap)> GetPropertyMaps(IDataReader reader, DbSettings dbSettings, Type classType)
        {
            var columns = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                columns[i] = reader.GetName(i);

            var map = dbSettings.MapCaching.GetOrCreate(classType);
            for (int i = 0; i < columns.Length; i++)
            {
                var column = columns[i];
                var propertyMap = map.GetColumn(column);
                if (propertyMap is not null)
                    yield return (i, propertyMap);
            }
        }
    }
}
