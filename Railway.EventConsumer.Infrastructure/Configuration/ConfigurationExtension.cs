using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Railway.EventConsumer.Infrastructure.Messaging;

namespace Railway.EventConsumer.Infrastructure.Configuration
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddHostedService<RabbitMqConsumer>();
            return services;
        }
    }
}
