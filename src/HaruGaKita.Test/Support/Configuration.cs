using System.IO;
using Microsoft.Extensions.Configuration;

namespace HaruGaKita.Test.Support
{
    public static class Configuration
    {
        public static string ProjectRoot = Directory.GetCurrentDirectory();

        public static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
              .SetBasePath(ProjectRoot)
              .AddJsonFile("appsettings.json")
              .Build();
        }
    }
}