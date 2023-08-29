using HexagonalArch.Application.Events.IntegrationEvents;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;
using HexagonalArch.Domain.Events;

namespace HexagonalArch.Application.Events.DomainEventHandlers;

public class AccumuatedAmountChallengeCreatedEventHandler : DomainEventHandler<AccumuatedAmountChallengeCreated>
{
    private readonly IIntegrationEventService _integrationEventService;
    private readonly IGuidProvider _guidProvider;

    public AccumuatedAmountChallengeCreatedEventHandler(IIntegrationEventService integrationEventService, IGuidProvider guidProvider)
    {
        _integrationEventService = integrationEventService;
        _guidProvider = guidProvider;
    }

    public override async Task Handle(DomainEventNotification<AccumuatedAmountChallengeCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.Event;

        
        var integrationEvent = new IntegrationStartedEvent(_guidProvider.NewId());

        await _integrationEventService.SaveAsync(integrationEvent, cancellationToken);
    }
}
