using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public interface ICollectedBalanceChallengeRepository : IRepository
{
    Task<IList<CollectedBalanceChallenge>> GetChallengesByUserId(Guid userId);
}
