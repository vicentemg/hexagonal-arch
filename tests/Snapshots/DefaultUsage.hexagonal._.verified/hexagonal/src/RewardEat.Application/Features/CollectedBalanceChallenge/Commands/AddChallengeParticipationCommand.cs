using RewardEat.Domain.SeedWork;

namespace RewardEat.Application.Features.CollectedBalanceChallenge.Commands;

public record class AddChallengeParticipationCommand(
    Guid UserId,
    Guid TransactionId,
    decimal Amount,
    DateTime OperationDateTime) : IRequest<Result<AddChallengeParticipationCommand.Response>>
{
    public record Response(IEnumerable<Guid> ParticipationIds);
}
