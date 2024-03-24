using HexagonalArch.Application;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HexagonalArch.Adapter.BusProcess;

public static class BusProcessModule
{
    public static IServiceCollection AddEventBusPublisher(this IServiceCollection services)
    {
        services
            .ConfigureApplicationModule()
            .AddMassTransit();

        return services;
    }

    private static IServiceCollection AddMassTransit(this IServiceCollection services)
    {
        return services
            .AddMassTransit(options =>
            {
                options.UsingRabbitMq((context, cfg) => { cfg.ConfigureEndpoints(context); });
            });
    }
}
