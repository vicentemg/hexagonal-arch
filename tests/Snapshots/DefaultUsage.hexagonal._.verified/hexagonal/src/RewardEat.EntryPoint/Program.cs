using RewardEat.Adapter.Cache;
using RewardEat.Adapter.Http;
using RewardEat.Adapter.Persistence;
using RewardEat.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureApplicationModule()
    .ConfigureHttpAdapter()
    .ConfigureCacheAdapter()
    .ConfigurePersistenceAdapter();

var app = builder.Build();

app.UseHttpAdapter();

app.Run();
