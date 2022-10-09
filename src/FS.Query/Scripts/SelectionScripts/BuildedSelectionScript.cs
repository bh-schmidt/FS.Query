using FS.Query.Scripts.SelectionScripts.Filters.Comparables;
using FS.Query.Scripts.SelectionScripts.Parameters;
using FS.Query.Scripts.SelectionScripts.Selects;
using FS.Query.Settings;
using System;
using System.Data;
using System.Text;

namespace FS.Query.Scripts.SelectionScripts
{
    public class BuildedSelectionScript
    {
        private readonly string buildedScript;

        public BuildedSelectionScript(
            string buildedScript,
            Select[] selectColumns,
            ScriptParameters scriptParameters)
        {
            this.buildedScript = buildedScript;
            SelectColumns = selectColumns;
            ScriptParameters = scriptParameters;
        }

        public Select[] SelectColumns { get; }
        public ScriptParameters ScriptParameters { get; }

        public IDbCommand PrepareCommand(
            DbSettings dbSettings,
            ScriptParameters scriptParameters,
            IDbConnection dbConnection)
        {
            var command = dbConnection.CreateCommand();

            command.CommandText = AddParameters(dbSettings, scriptParameters, command);
            command.CommandTimeout = 30;
            command.CommandType = CommandType.Text;

            return command;
        }

        private string AddParameters(DbSettings dbSettings, ScriptParameters scriptParameters, IDbCommand command)
        {
            var scriptBuilder = new StringBuilder(buildedScript);

            foreach (var parameter in ScriptParameters.Parameters)
            {
                var parameterToInject = scriptParameters.Get(parameter.Key);
                if (parameterToInject is null || parameter.Value != parameterToInject)
                    throw new Exception("The script parameters differ.");

                if (parameterToInject is ComparableValue comparableValue)
                {
                    var commandParameter = command.CreateParameter();
                    comparableValue.AddParameter(parameter.Key, commandParameter, dbSettings);
                    command.Parameters.Add(commandParameter);
                    continue;
                }
                else if (parameterToInject is ComparableEnumerable comparableEnumerable)
                {
                    var values = comparableEnumerable.BuildAsString(dbSettings).ToString();
                    scriptBuilder.Replace(parameter.Key, values);
                    continue;
                }
            }

            return scriptBuilder.ToString();
        }
    }
}
