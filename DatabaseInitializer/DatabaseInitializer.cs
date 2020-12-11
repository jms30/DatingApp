using System;
using System.Data;
using Npgsql;

namespace DatingApp.DatabaseInitializer
{
    public class DatabaseInitializer
    {
        private readonly NpgsqlConnection _dbConnection;

        public DatabaseInitializer(IDbConnection connection) {
            this._dbConnection = (NpgsqlConnection)connection;
        }

        public void InitializeWithDefaultDatabase(string databaseName) {
            var databases = databaseName.Split(",");
            try {
                this._dbConnection.Open();

                foreach (var db in databases) {
                    if(!string.IsNullOrEmpty(db)) {
                        var existingDbCommand = this._dbConnection.CreateCommand();
                        existingDbCommand.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{db}'";
                        var result = existingDbCommand.ExecuteScalar();

                        if (result == null ) {
                            var command = this._dbConnection.CreateCommand();
                            command.CommandText = $"CREATE DATABASE \"{db}\"";
                            command.ExecuteNonQuery();
                            Console.WriteLine($"Postgres: Created database {db}");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Some exception happened while trying to create default database. " + ex);
            }
            finally {
                this._dbConnection.Close();
            }
            Console.WriteLine("Postgres: Step complete.");
        }

        public void Initialize() {
            try
            {
                var evolve = new Evolve.Evolve(this._dbConnection)
                {
                    EmbeddedResourceAssemblies = new [] { typeof(DatabaseInitializer).Assembly },
                    EmbeddedResourceFilters = new [] { "DatabaseInitializer.db.migrations" },
                };

                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database migration failed.", ex);
                throw;
            }

        }
    }
}