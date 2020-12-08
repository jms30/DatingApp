
using System;
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
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public static void Main(string[] args)
        {
            InitializeDatabase();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        private static void InitializeDatabase() {
            RepoDb.PostgreSqlBootstrap.Initialize();
            var dbConnection = new NpgsqlConnection(Configuration["Database:ConnectionString"]);
            var dbInitializer = new DatabaseInitializer.DatabaseInitializer(dbConnection);
        }
    }
}
