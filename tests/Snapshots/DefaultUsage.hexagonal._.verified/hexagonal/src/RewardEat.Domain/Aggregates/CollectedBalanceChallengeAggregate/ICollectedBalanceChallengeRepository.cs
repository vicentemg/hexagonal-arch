using RewardEat.Domain.SeedWork;

namespace RewardEat.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public interface ICollectedBalanceChallengeRepository : IRepository
{
    Task<IList<CollectedBalanceChallenge>> GetChallengesByUserId(Guid userId);
}
