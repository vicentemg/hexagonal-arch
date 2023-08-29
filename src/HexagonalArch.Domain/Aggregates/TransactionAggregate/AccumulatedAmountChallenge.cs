using HexagonalArch.Domain.Events;
using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Aggregates.TransactionAggregate;

public class AccumulatedAmountChallenge : Entity, IAggregateRoot
{
    AccumulatedAmountChallenge(
        Guid id,
        AccumulatedAmountConstraint constraint,
        DateTime createdDateTime) : base(id)
    {
        Constraint = constraint;
        CreatedDateTime = createdDateTime;
        AddDomainEvent(
            new AccumuatedAmountChallengeCreated(id)
        );

    }

    public static Result<AccumulatedAmountChallenge> Create(
        Guid id,
        AccumulatedAmountConstraint constraint,
        DateTime createdDateTime
        )
    {
        var challenge = new AccumulatedAmountChallenge(id, constraint, createdDateTime);
        return Result<AccumulatedAmountChallenge>.Success(challenge);
    }

    public AccumulatedAmountConstraint Constraint { get; }

    public DateTime CreatedDateTime { get; }

    public bool Active { get; } = true;
}
