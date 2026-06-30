using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Railway.EventConsumer.Application.Handlers;
using Railway.EventConsumer.Infrastructure.Messaging;

namespace Railway.EventConsumer.Infrastructure.Configuration
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IMessageDispatcher, MessageDispatcher>();
            services.AddHostedService<RabbitMqConsumer>();
            return services;
        }
    }
}
