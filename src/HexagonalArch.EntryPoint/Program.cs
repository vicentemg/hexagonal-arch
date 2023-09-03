using HexagonalArch.Adapter.Cache;
using HexagonalArch.Adapter.Persistance;
using HexagonalArch.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureApplicationModule()
    .ConfigureHttpAdapter()
    .ConfigureCacheAdapter()
    .ConfigurePersistanceAdapter();

var app = builder.Build();

app.UseHttpAdapter();

app.Run();