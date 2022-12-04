using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Db
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigureDb(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<InfluxDbOptions>(configuration.GetSection("InfluxDB"))
                .AddTransient<InfluxDBClientProvider>()
                .AddTransient<IFluxQueryBuilder>(c => new FluxQueryBuilder(
                    c.GetService<IOptions<InfluxDbOptions>>(),
                    c.GetService<InfluxDBClientProvider>().Get()
                ))
                .AddTransient<IEnvSensorsStore, EnvSensorsStore>(c =>
                    new EnvSensorsStore(
                        c.GetService<IOptions<InfluxDbOptions>>(),
                        c.GetService<InfluxDBClientProvider>().Get()
                    ))
                .AddTransient<IAlertsStore, AlertsStore>(c =>
                    new AlertsStore(
                        c.GetService<IOptions<InfluxDbOptions>>(),
                        c.GetService<InfluxDBClientProvider>().Get()
                    ));
        }

    }
}
