﻿using FS.Query.Builders;
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

        public TableSelectBuilder<TTable> FromTable<TTable>(string alias) where TTable : new()
        {
            var table = new Table(typeof(TTable), alias);
            var script = new Script(table);
            return new TableSelectBuilder<TTable>(table, script, this);
        }

        public IDbConnection GetConnection()
        {
            if (DbSettings.Connection.CreateConnection is null)
                throw new ArgumentException("The function to create connection wasn't set.");

            dbConnection ??= DbSettings.Connection.CreateConnection.Invoke(serviceProvider);

            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();

            return dbConnection;
        }

        public IDbConnection GetUniqueConnection()
        {
            if (DbSettings.Connection.CreateConnection is null)
                throw new ArgumentException("The function to create connection wasn't set.");

            var connection = DbSettings.Connection.CreateConnection.Invoke(serviceProvider);

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            uniqueConnections.AddLast(connection);

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