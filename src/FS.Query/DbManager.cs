using FS.Query.Builders;
using FS.Query.Helpers;
using FS.Query.Scripts.SelectionScripts;
using FS.Query.Scripts.SelectionScripts.Sources;
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
        private LinkedList<IDbConnection>? uniqueConnections;

        public DbManager(DbSettings dbSettings, IServiceProvider serviceProvider)
        {
            DbSettings = dbSettings;
            this.serviceProvider = serviceProvider;
        }

        public DbSettings DbSettings { get; }

        public SelectionByTableBuilder<TTable> FromTable<TTable>(string alias)
        {
            var table = new Table(typeof(TTable), alias);
            var selectionScript = new SelectionScript(table);
            return new SelectionByTableBuilder<TTable>(table, selectionScript, this);
        }

        public SelectionBuilder FromScript(string alias, string injection)
        {
            var scriptInjection = new ScriptInjection(alias, injection);
            var selectionScript = new SelectionScript(scriptInjection);
            return new SelectionBuilder(scriptInjection, selectionScript, this);
        }

        public IDbConnection Connection => dbConnection ??= CreateConnection();

        public IDbConnection UniqueConnection
        {
            get
            {
                var connection = CreateConnection();
                uniqueConnections ??= new();
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

            if (uniqueConnections is not null)
                foreach (var connection in uniqueConnections)
                    Try.Execute(() => connection?.Dispose());

            GC.SuppressFinalize(this);
        }

        ~DbManager()
        {
            Dispose();
            dbConnection = null;
            uniqueConnections = null;
        }
    }
}
