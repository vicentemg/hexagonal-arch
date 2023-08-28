using Microsoft.Extensions.DependencyInjection;

namespace HexagonalArch.Adapter.Persistance;

public static class PersistanceModule
{
    public static IServiceCollection ConfigurePersistanceAdapter(this IServiceCollection services)
    {
        return services;
    }

}
