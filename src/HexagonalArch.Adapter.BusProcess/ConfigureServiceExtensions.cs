using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HexagonalArch.Adapter.BusProcess;

public static class ConfigureServiceExtensions
{
    public static IServiceCollection AddEventBusPublisher(this IServiceCollection services)
    {
        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
        });

        return services;
    }

    public static IServiceCollection AddEventBusConsumer(this IServiceCollection services)
    {
        return services;
    }
}
