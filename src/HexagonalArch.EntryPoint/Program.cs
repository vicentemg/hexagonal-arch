using HexagonalArch.Adapter.Cache;
using HexagonalArch.Adapter.Http;
using HexagonalArch.Adapter.Persistence;
using HexagonalArch.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureApplicationModule()
    .ConfigureHttpAdapter()
    .ConfigureCacheAdapter()
    .ConfigurePersistenceAdapter();

var app = builder.Build();

app.UseHttpAdapter();

app.Run();
