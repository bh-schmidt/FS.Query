using FS.Query.Scripts.Parameters;
using FS.Query.Settings;
using System;
using System.Text;

namespace FS.Query.Scripts
{
    public class BuildedScript
    {
        private readonly string script;
        private readonly ScriptParameters scriptParameters;

        public BuildedScript(string Script, ScriptParameters scriptParameters)
        {
            script = Script;
            this.scriptParameters = scriptParameters;
        }

        public string GetScript(DbSettings dbSettings, ScriptParameters scriptParameters)
        {
            var scriptBuilder = new StringBuilder(script);

            foreach (var parameter in this.scriptParameters.Parameters)
            {
                var parameterToInject = scriptParameters.Get(parameter.Key);
                if (parameterToInject is null || parameter.Value != parameterToInject)
                    throw new Exception("The script parameters differ.");

                var buildedParameter = parameterToInject.BuildParameter(dbSettings);
                scriptBuilder.Replace(parameter.Key, buildedParameter.ToString());
            }

            return scriptBuilder.ToString();
        }
    }
}
