
using System;
using System.Threading;
using DatingApp.DatabaseInitializer;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static int Main(string[] args)
        {
            try {
                Thread.Sleep(2000);
                InitializeDatabase();
                Console.WriteLine("Successfully initialize and migrated database.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception) {
                Console.WriteLine("Host terminated unexpectedly.");
                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        private static void InitializeDatabase() {
            RepoDb.PostgreSqlBootstrap.Initialize();
            
            var ddlconnectionstr = Configuration["Database:DdlConnectionString"];
            var connectionstr = Configuration["Database:ConnectionString"];
            var databasenames = Configuration["Database:Names"];

            DatabaseInitializer.DatabaseInitializer dbInitializer = null;
            try {
                var dbConnection = new NpgsqlConnection(ddlconnectionstr);
                dbInitializer = new DatabaseInitializer.DatabaseInitializer(dbConnection);
                Console.WriteLine($"Trying to create default database with connection string : {dbConnection.ConnectionString}\n " +
                                $"and database names: {databasenames}" );
                dbInitializer.InitializeWithDefaultDatabase(databasenames);
                Console.WriteLine("Default Database Creation Successful");
            }
            catch (Exception ex) {
                Console.WriteLine("Exception raised while instantiating default database: " + ex);
                throw;
            }

            try {
                var dbConnection = new NpgsqlConnection(connectionstr);
                dbInitializer = new DatabaseInitializer.DatabaseInitializer(dbConnection);
                dbInitializer.Initialize();
                Console.WriteLine("Database Migration Successful");
            }
            catch (Exception ex) {
                Console.WriteLine("Exception raised while migrating in database: " + ex);
                throw;
            }
        }
    }
}
