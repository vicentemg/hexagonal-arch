using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using HexagonalArch.Application.Providers;
using HexagonalArch.Domain.SeedWork;
using Microsoft.Extensions.Logging;
using MediatR;

namespace HexagonalArch.Application.Features.CollectedBalanceChallenge.Commands;

public class AddAmountParticipationCommandHandler : IRequestHandler<AddAmountParticipationCommand, Result<AddAmountParticipationCommand.Response>>
{
    private readonly IGuidProvider _guidProvider;
    private readonly ICollectedBalanceChallengeRepository _repository;
    private readonly ILogger<AddAmountParticipationCommandHandler> _logger;
    public AddAmountParticipationCommandHandler(
        ICollectedBalanceChallengeRepository repository,
        ILogger<AddAmountParticipationCommandHandler> logger,
        IGuidProvider guidProvider)
    {
        _logger = logger;
        _repository = repository;
        _guidProvider = guidProvider;
    }

    public async Task<Result<AddAmountParticipationCommand.Response>> Handle(AddAmountParticipationCommand request, CancellationToken cancellationToken)
    {
        var newParticipations = new List<Guid>();

        var challenges = await _repository.GetChallengesByUserId(request.UserId);

        foreach (var challenge in challenges)
        {
            var newParticipationId = _guidProvider.NewId();

            var participation = CollectedBalanceChallengeParticipation.Create(
                   newParticipationId,
                   request.UserId,
                   challenge.Id,
                   request.TransactionId,
                   request.Amount,
                   request.OperationDateTime
               );

            var result = challenge.AddParticipation(participation);

            if (!result.IsSuccess)
            {
                return Result<AddAmountParticipationCommand.Response>.Failure(result.Errors);
            }

            newParticipations.Add(newParticipationId);
        }

        var unitOfWork = _repository.UnitOfWork;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddAmountParticipationCommand.Response(newParticipations);
    }
}
