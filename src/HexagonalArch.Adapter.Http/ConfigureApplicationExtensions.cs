public static class ConfigureApplicationExtensions
{
    public static WebApplication ConfigureHttpAdapter(this WebApplication app)
    {
        app
         .MapGet("/", () => "Hello!");
        
        return app;
    }
}