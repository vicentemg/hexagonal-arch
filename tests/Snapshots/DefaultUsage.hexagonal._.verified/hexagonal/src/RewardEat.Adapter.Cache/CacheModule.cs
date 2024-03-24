using Microsoft.Extensions.DependencyInjection;

namespace RewardEat.Adapter.Cache;

public static class CacheModule
{
    public static IServiceCollection ConfigureCacheAdapter(this IServiceCollection services)
    {
        return services;
    }
}
