using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Events;

public record AccomplishedCollectedBalanceChallenge(Guid ChallengeId, Guid UserId) : IDomainEvent
{
}