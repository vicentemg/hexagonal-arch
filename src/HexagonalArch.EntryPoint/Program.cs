var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddHttpAdapter();

var app = builder.Build();

app.ConfigureHttpAdapter();

app.Run();