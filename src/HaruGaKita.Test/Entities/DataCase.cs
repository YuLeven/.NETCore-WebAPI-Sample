using System.IO;
using HaruGaKita.Infrastructure.Data;
using HaruGaKita.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HaruGaKita.Test
{
    public static class DataCase
    {
        public static IUserRepository _userRepository { get; }
        public static string _connectionString { get; }
        static readonly IWebHost _testHost;
        static readonly IConfigurationRoot _configuration;
        static readonly HaruGaKitaContext _databaseContext;

        static DataCase()
        {
            var projectDir = Directory.GetCurrentDirectory();
            _configuration = new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.json")
                    .Build();
            _connectionString = _configuration.GetConnectionString("HaruGaKitaDB");

            _testHost = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseContentRoot(projectDir)
                .UseConfiguration(_configuration)
                .UseStartup<Startup>()
                .Build();

            _databaseContext = _testHost.Services.GetService(typeof(HaruGaKitaContext)) as HaruGaKitaContext;
            _userRepository = _testHost.Services.GetService(typeof(IUserRepository)) as UserRepository;
            MigrateDatabase();
        }

        static void MigrateDatabase()
        {
            if (_databaseContext.Database.EnsureCreated())
            {
                _databaseContext.Database.Migrate();
            }
        }
    }
}