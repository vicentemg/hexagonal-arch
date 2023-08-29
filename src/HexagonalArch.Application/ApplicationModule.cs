using System.Reflection;
using System.Reflection.Metadata;
using HexagonalArch.Application.Events.DomainEventHandlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HexagonalArch.Application;
public static class ApplicationModule
{
    public static IServiceCollection ConfigureApplicationModule(this IServiceCollection services)
    {
        var assembly = typeof(IApplicationAssemblyMarker).Assembly;

        return services
                .AddCustomMediatR(assembly);
    }

    internal static IServiceCollection AddCustomMediatR(this IServiceCollection services, Assembly assembly)
    {

        services
            .AddMediatR(options => options.RegisterServicesFromAssembly(assembly))
            .AddDomainEventHandlers(assembly);

        return services;
    }

    private static void AddDomainEventHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
                    .Where(type => !type.IsAbstract && type.BaseType?.IsGenericType == true && type.BaseType.GetGenericTypeDefinition() == typeof(DomainEventHandler<>));

        foreach (var handlerType in handlerTypes)
        {
            services.AddTransient(typeof(INotificationHandler<>).MakeGenericType(handlerType.BaseType.GetGenericArguments()[0]), handlerType);
        }
    }
}