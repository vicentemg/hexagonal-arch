using HexagonalArch.Application.Events.Integration;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;
using HexagonalArch.Domain.Events;
using MediatR;

namespace HexagonalArch.Application.Events.Domain.Handlers;

public class AccumuatedAmountChallengeCreatedEventHandler : INotificationHandler<CollectedBalanceChallengeCreated>
{
    private readonly IOutBoxMessageService _outBoxMessageService;
    private readonly IGuidProvider _guidProvider;

    public AccumuatedAmountChallengeCreatedEventHandler(
        IOutBoxMessageService outBoxMessageService,
        IGuidProvider guidProvider)
    {
        _outBoxMessageService = outBoxMessageService;
        _guidProvider = guidProvider;
    }

    public async Task Handle(CollectedBalanceChallengeCreated @event, CancellationToken cancellationToken)
    {

        var integrationEvent = new EarnedPoints(_guidProvider.NewId());

        await _outBoxMessageService.AddIntegrationEventAsync(integrationEvent, cancellationToken);
        // return Task.CompletedTask;
    }
}
