using System.Reflection;
using HexagonalArch.Application.Events.Domain;
using HexagonalArch.Application.Features;
using HexagonalArch.Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace HexagonalArch.Application;
public static class ApplicationModule
{
    public static IServiceCollection ConfigureApplicationModule(this IServiceCollection services)
    {
        var assembly = typeof(IApplicationAssemblyMarker).Assembly;

        return services
               .AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
    }
}