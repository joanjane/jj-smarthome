using Microsoft.Extensions.Configuration;

namespace JJ.SmartHome.Tests.Utils
{
    public class ConfigBuilder
    {
        public static IConfiguration Build()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json")
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
    }
}
