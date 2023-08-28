using Microsoft.Extensions.DependencyInjection;

namespace HexagonalArch.Application;
public static class ApplicationModule
{
    public static IServiceCollection ConfigureApplicationModule(this IServiceCollection services)
    {
        return services;
    }
}