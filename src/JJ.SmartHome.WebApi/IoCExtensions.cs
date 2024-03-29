﻿using JJ.SmartHome.Core;
using JJ.SmartHome.Db;
using JJ.SmartHome.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JJ.SmartHome.WebApi
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .ConfigureCore(configuration)
                .ConfigureDb(configuration)
                .ConfigureMailing(configuration)
                ;
        }
    }
}
