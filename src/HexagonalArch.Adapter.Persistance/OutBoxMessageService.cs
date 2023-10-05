using HexagonalArch.Application.Events.Integration;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;

namespace HexagonalArch.Adapter.Persistance;

public class OutBoxMessageService : IOutBoxMessageService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IGuidProvider _guidProvider;

    public OutBoxMessageService(IDateTimeProvider dateTimeProvider, IGuidProvider guidProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _guidProvider = guidProvider;
    }

    public Task AddIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}