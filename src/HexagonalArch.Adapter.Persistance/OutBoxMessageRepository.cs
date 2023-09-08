using System.Text.Json;
using HexagonalArch.Adapter.Persistance.Entities;
using HexagonalArch.Application.Events.Integration;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;

namespace HexagonalArch.Adapter.Persistance;

public class OutBoxMessageRepository : IOutBoxMessageService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IGuidProvider _guidProvider;

    public OutBoxMessageRepository(IDateTimeProvider dateTimeProvider, IGuidProvider guidProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _guidProvider = guidProvider;
    }

    public Task AddIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        var outBoxMessage = new OutBoxMessage
        {
            Id = _guidProvider.NewId(),
            Type = integrationEvent.GetType().AssemblyQualifiedName,
            EventData = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()),
            OccurredOnUtcTime = _dateTimeProvider.UtcNow
        };

        return Task.CompletedTask;

    }
}
