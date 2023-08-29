using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Events;

public record AccumuatedAmountChallengeCreated(Guid ChallengeId) : IDomainEvent
{
}