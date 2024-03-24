using RewardEat.Domain.SeedWork;

namespace RewardEat.Application.Features.CollectedBalanceChallenge.Queries;

public record GetChallengeParticipationQuery(Guid Id) : IRequest<Result<GetChallengeParticipationQuery.Response>>
{
    public record Response(Guid Id, Guid TransactionId, DateTime OccurredOn);
}
