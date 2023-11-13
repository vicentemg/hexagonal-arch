using Microsoft.Extensions.DependencyInjection;

namespace HexagonalArch.Adapter.Persistence;

public static class PersistenceModule
{
    public static IServiceCollection ConfigurePersistenceAdapter(this IServiceCollection services)
    {
        return services;
    }
}