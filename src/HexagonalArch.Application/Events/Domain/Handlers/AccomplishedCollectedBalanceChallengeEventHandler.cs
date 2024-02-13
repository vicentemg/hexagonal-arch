using HexagonalArch.Application.Events.Integration.Events;
using HexagonalArch.Application.Features.CollectedBalanceChallenge.Commands;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;
using HexagonalArch.Domain.Events;
using Microsoft.Extensions.Logging;

namespace HexagonalArch.Application.Events.Domain.Handlers;

public class
    AccomplishedCollectedBalanceChallengeEventHandler : IDomainEventHandler<AccomplishedCollectedBalanceChallenge>
{
    private readonly IGuidProvider _guidProvider;
    private readonly ILogger<AddChallengeParticipationCommandHandler> _logger;
    private readonly IOutBoxMessageService _outBoxMessageService;

    public AccomplishedCollectedBalanceChallengeEventHandler(
        IOutBoxMessageService outBoxMessageService,
        ILogger<AddChallengeParticipationCommandHandler> logger,
        IGuidProvider guidProvider)
    {
        _outBoxMessageService = outBoxMessageService;
        _logger = logger;
        _guidProvider = guidProvider;
    }

    public async Task Handle(AccomplishedCollectedBalanceChallenge @event, CancellationToken cancellationToken)
    {
        var integrationEvent = new EarnedPoints(_guidProvider.NewId(), @event.UserId, 10);

        await _outBoxMessageService.AddIntegrationEventAsync(integrationEvent, cancellationToken);
    }
}
