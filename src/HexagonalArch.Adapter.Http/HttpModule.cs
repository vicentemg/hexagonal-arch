using HexagonalArch.Application;
public static class HttpModule
{
    public static IServiceCollection ConfigureHttpAdapter(this IServiceCollection services)
    {
        return services;
    }

    public static WebApplication UseHttpAdapter(this WebApplication app)
    {
        app
         .MapGet("/", () => "Hello!");

        return app;
    }
}
