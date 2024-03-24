using RewardEat.Adapter.Http.Endpoints;

namespace RewardEat.Adapter.Http;

public static class HttpModule
{
    public static IServiceCollection ConfigureHttpAdapter(this IServiceCollection services)
    {
        return services;
    }

    public static WebApplication UseHttpAdapter(this WebApplication app)
    {
        app.MapChallengeParticipationEndPoints();

        return app;
    }
}
