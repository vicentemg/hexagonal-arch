using System.Reflection;
using RewardEat.Application.Events;
using RewardEat.Application.Events.Domain;
using RewardEat.Application.Events.Integration;
using Microsoft.Extensions.DependencyInjection;

namespace RewardEat.Application;

public static class ApplicationModule
{
    public static IServiceCollection ConfigureApplicationModule(this IServiceCollection services)
    {
        var assembly = typeof(IApplicationAssemblyMarker).Assembly;

        return services
            .AddEventHandlers(assembly)
            .AddTransient<IIntegrationEventDispatcher, IntegrationEventDispatcher>()
            .AddTransient<IEventDispatcher, DomainEventDispatcher>();
    }

    internal static IServiceCollection AddEventHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlers = assembly
            .GetTypes()
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false }
                && type.GetInterfaces()
                    .Any(@interface =>
                        @interface.IsGenericType
                        && (@interface.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                            || @interface.GetGenericTypeDefinition() ==
                            typeof(IIntegrationEventHandler<>))
                    )
            );

        foreach (var handlerImplementation in handlers)
        {
            var interfaceType = handlerImplementation
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType
                                     && (i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                                         || i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
                );

            if (interfaceType is null) continue;

            services.AddTransient(interfaceType, handlerImplementation);
        }

        return services;
    }
}
