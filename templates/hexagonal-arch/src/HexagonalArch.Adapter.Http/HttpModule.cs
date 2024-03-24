using HexagonalArch.Adapter.Http.Endpoints;

namespace HexagonalArch.Adapter.Http;

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
