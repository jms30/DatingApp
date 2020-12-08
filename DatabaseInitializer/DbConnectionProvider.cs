using System.Data;
using Npgsql;

namespace DatingApp.DatabaseInitializer
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly string _connectionString = "";

        public DbConnectionProvider(string connectionString) {
            this._connectionString = connectionString;
        }

        public string ConnectionString => this._connectionString;

        public IDbConnection GetDbConnection() {
            var newConnection = new NpgsqlConnection(this._connectionString);
            return newConnection;
        }
    }
}
