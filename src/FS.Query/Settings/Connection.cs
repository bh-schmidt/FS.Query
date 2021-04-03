using System;
using System.Data;

namespace FS.Query.Settings
{
    public class Connection
    {
        public virtual Func<IServiceProvider, IDbConnection>? CreateConnection { get; }

        public Connection(Func<IServiceProvider, IDbConnection> createConnection)
        {
            CreateConnection = createConnection;
        }
    }
}
