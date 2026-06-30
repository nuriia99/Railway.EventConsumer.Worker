using Microsoft.Extensions.DependencyInjection;
using Railway.EventConsumer.Application.Handlers;

namespace Railway.EventConsumer.Application.Configuration
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
            {
                services.AddScoped<IMessageHandler, MessageTestHandler>();

                return services;
            }
    }
}
