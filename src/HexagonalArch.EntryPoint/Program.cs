using HexagonalArch.Adapter.Cache;
using HexagonalArch.Adapter.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .ConfigureHttpAdapter()
    .ConfigureCacheAdapter()
    .ConfigurePersistanceAdapter();

var app = builder.Build();

app.UseHttpAdapter();

app.Run();