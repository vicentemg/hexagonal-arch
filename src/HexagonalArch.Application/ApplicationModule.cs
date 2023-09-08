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