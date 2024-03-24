using RewardEat.Domain.SeedWork;

namespace RewardEat.Domain.Events;

public record AccomplishedCollectedBalanceChallenge(Guid ChallengeId, Guid UserId) : IDomainEvent
{
}
