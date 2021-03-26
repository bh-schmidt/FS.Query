using System;
using System.Data;

namespace FS.Query.Settings
{
    public class Connection
    {
        public Func<IServiceProvider, IDbConnection>? CreateConnection { get; }

        public Connection(Func<IServiceProvider, IDbConnection> createConnection)
        {
            CreateConnection = createConnection;
        }
    }
}
