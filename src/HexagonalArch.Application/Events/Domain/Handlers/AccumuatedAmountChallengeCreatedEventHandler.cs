using HexagonalArch.Application.Events.Integration;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;
using HexagonalArch.Domain.Events;
using MediatR;

namespace HexagonalArch.Application.Events.Domain.Handlers;

public class AccumuatedAmountChallengeCreatedEventHandler : INotificationHandler<CollectedBalanceChallengeCreated>
{
    private readonly IOutBoxMessageRepository _integrationEventService;
    private readonly IGuidProvider _guidProvider;

    public AccumuatedAmountChallengeCreatedEventHandler(
        IOutBoxMessageRepository integrationEventService,
        IGuidProvider guidProvider)
    {
        _integrationEventService = integrationEventService;
        _guidProvider = guidProvider;
    }

    public async Task Handle(CollectedBalanceChallengeCreated @event, CancellationToken cancellationToken)
    {

        var integrationEvent = new PointsEarned(_guidProvider.NewId());

        await _integrationEventService.AddIntegrationEventAsync(integrationEvent, cancellationToken);
        // return Task.CompletedTask;
    }
}
