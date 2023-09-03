using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Events;

public record CollectedBalanceChallengeCreated(Guid ChallengeId) : IDomainEvent
{
}