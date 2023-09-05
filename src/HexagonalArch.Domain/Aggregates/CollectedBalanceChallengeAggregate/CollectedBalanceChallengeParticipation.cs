using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public class CollectedBalanceChallengeParticipation : Entity
{
    public CollectedBalanceChallengeParticipation(
        Guid id,
        Guid userId,
        Guid challengeId,
        Guid transactionId,
        decimal amount,
        DateTime occurredOn)
    {
        Id = id;
        Amount = amount;
        UserId = userId;
        OccurredOn = occurredOn;
        ChallengeId = challengeId;
        TransactionId = transactionId;
    }

    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid ChallengeId { get; }
    public Guid TransactionId { get; }
    public decimal Amount { get; }
    public DateTime OccurredOn { get; }
    public bool IsWinner { get; private set; } = false;

    public void SetAsWinner()
    {
        IsWinner = true;
    }

    public static Result<CollectedBalanceChallengeParticipation> Create(
        Guid id,
        Guid userId,
        Guid challengeId,
        Guid transactionId,
        decimal amount,
        DateTime occuredOn)
    {
        return new CollectedBalanceChallengeParticipation(id, userId, challengeId, transactionId, amount, occuredOn);
    }
}