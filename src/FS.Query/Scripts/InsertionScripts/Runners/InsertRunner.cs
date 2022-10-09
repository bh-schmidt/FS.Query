using System.Drawing;

namespace FS.Query.Scripts.InsertionScripts.Runners
{
    public class InsertRunner
    {
        private readonly BuildedInsertionScript buildedInsertionScript;
        private readonly DbManager dbManager;

        public InsertRunner(BuildedInsertionScript buildedInsertionScript, DbManager dbManager)
        {
            this.buildedInsertionScript = buildedInsertionScript;
            this.dbManager = dbManager;
        }
    }
}
