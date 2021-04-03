using FS.Query.Builders;
using FS.Query.Helpers;
using FS.Query.Scripts.Sources;
using FS.Query.Settings;
using System;
using System.Collections.Generic;
using System.Data;

namespace FS.Query
{
    public class DbManager : IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private IDbConnection? dbConnection;
        private LinkedList<IDbConnection> uniqueConnections = new();

        public DbManager(DbSettings dbSettings, IServiceProvider serviceProvider)
        {
            DbSettings = dbSettings;
            this.serviceProvider = serviceProvider;
        }

        public DbSettings DbSettings { get; }

        public SelectionByTableBuilder<TTable> FromTable<TTable>(string alias)
        {
            var table = new Table(typeof(TTable), alias);
            var script = new SelectionScript(table);
            return new SelectionByTableBuilder<TTable>(table, script, this);
        }

        public SelectionByScriptInjectionBuilder FromScript(string alias, string injection)
        {
            var scriptInjection = new ScriptInjection(alias, injection);
            var script = new SelectionScript(scriptInjection);
            return new SelectionByScriptInjectionBuilder(scriptInjection, script, this);
        }

        public IDbConnection Connection => dbConnection ??= CreateConnection();

        public IDbConnection UniqueConnection
        {
            get
            {
                var connection = CreateConnection();
                uniqueConnections.AddLast(connection);
                return connection;
            }
        }

        private IDbConnection CreateConnection()
        {
            if (DbSettings?.Connection?.CreateConnection is null)
                throw new ArgumentException("The function to create connection wasn't set.");

            var connection = DbSettings.Connection.CreateConnection(serviceProvider);

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            return connection;
        }

        public void Dispose()
        {
            Try.Execute(() => dbConnection?.Dispose());

            foreach (var connection in uniqueConnections)
                Try.Execute(() => connection?.Dispose());
        }

        ~DbManager()
        {
            Dispose();
            dbConnection = null;
            uniqueConnections.Clear();
        }
    }
}
