using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using HexagonalArch.Application.Providers;
using HexagonalArch.Domain.SeedWork;
using Microsoft.Extensions.Logging;
using MediatR;

namespace HexagonalArch.Application.Features.CollectedBalanceChallenge.Commands;

public class AddChallengeParticipationCommandHandler : IRequestHandler<AddChallengeParticipationCommand, Result<AddChallengeParticipationCommand.Response>>
{
    private readonly IGuidProvider _guidProvider;
    private readonly ICollectedBalanceChallengeRepository _repository;
    private readonly ILogger<AddChallengeParticipationCommandHandler> _logger;
    public AddChallengeParticipationCommandHandler(
        ICollectedBalanceChallengeRepository repository,
        ILogger<AddChallengeParticipationCommandHandler> logger,
        IGuidProvider guidProvider)
    {
        _logger = logger;
        _repository = repository;
        _guidProvider = guidProvider;
    }

    public async Task<Result<AddChallengeParticipationCommand.Response>> Handle(AddChallengeParticipationCommand request, CancellationToken cancellationToken)
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
                return Result<AddChallengeParticipationCommand.Response>.Failure(result.Errors);
            }

            newParticipations.Add(newParticipationId);
        }

        var unitOfWork = _repository.UnitOfWork;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddChallengeParticipationCommand.Response(newParticipations);
    }
}
