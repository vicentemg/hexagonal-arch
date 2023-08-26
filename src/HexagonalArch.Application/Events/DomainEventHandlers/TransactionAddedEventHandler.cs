using HexagonalArch.Application.Events.IntegrationEvents;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;
using HexagonalArch.Domain.Events;

namespace HexagonalArch.Application.Events.DomainEventHandlers;

public class TransactionAddedEventHandler : IDomainEventHandler<TransactionAddedEvent>
{
    private readonly IIntegrationEventService _integrationEventService;
    private readonly IGuidProvider _guidProvider;

    public TransactionAddedEventHandler(IIntegrationEventService integrationEventService, IGuidProvider guidProvider)
    {
        _integrationEventService = integrationEventService;
        _guidProvider = guidProvider;
    }

    public async Task HandleAsync(TransactionAddedEvent @event, CancellationToken cancellationToken)
    {
        var integrationEvent = new IntegrationStartedEvent(_guidProvider.NewId());

        await _integrationEventService.SaveAsync(integrationEvent, cancellationToken);
    }
}
