using RewardEat.Domain.SeedWork;

namespace RewardEat.Domain.Events;

public record CollectedBalanceChallengeCreated(Guid ChallengeId) : IDomainEvent
{
}
