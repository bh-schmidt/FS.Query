using FS.Query.Builders;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace FS.Query.Runners
{
    public static class ScriptRunner
    {
        public static IEnumerable<T> Run<T>(Script script, DatabaseManager databaseManager)
            where T : new()
        {
            var cacheKey = CacheKeyBuilder.Fact(script);
             //SelectFactory.GenerateSimple<TType>(propertyNames, DatabaseManager.DbSettings);


            var buildedScript = databaseManager
                .DbSettings
                .ScriptCache
                .GetOrCreate(
                cacheKey,
                e =>
                {
                    script.Build(databaseManager.DbSettings);
                    return script;
                });
            //if (Source is null)
            //    throw new ArgumentException("No source of data informed.");

            //ScriptFactory.Generate(Source, DatabaseManager.DbSettings);

            //var propertyNames = PropertyNames.ToHashSet();
            //var connection = DatabaseManager.GetConnection();
            //var command = connection.CreateCommand();

            //command.CommandText = script.ToString();
            //command.CommandTimeout = 30;
            //command.CommandType = CommandType.Text;

            //var reader = command.ExecuteReader();
            //var result = Read<TType>(script, reader);

            return default;
        }

        private static IEnumerable<T> Read<T>(Script script, IDataReader reader)
            where T : new()
        {
            while (reader.Read())
            {
                var obj = new T();
                for (int index = 0; index < reader.FieldCount; index++)
                {
                    var propertyMap = script.GetProperty(index);
                    propertyMap.Fill(obj, reader, index);
                }
                yield return obj;
            }
        }
    }
}
