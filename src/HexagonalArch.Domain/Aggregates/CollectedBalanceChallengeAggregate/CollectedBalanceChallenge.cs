using HexagonalArch.Domain.Events;
using HexagonalArch.Domain.Primitives;
using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public class CollectedBalanceChallenge : Entity, IAggregateRoot
{

    private List<CollectedBalanceChallengeParticipation> _participations = new();
    CollectedBalanceChallenge(
        Guid id,
        CollectedBalanceConstraint constraint,
        DateTime createdDateTime) : base(id)
    {
        CreatedDateTime = createdDateTime;
        Constraint = constraint;
        AddDomainEvent(
            new CollectedBalanceChallengeCreated(id)
        );

    }
    public CollectedBalanceConstraint Constraint { get; }

    public DateTime CreatedDateTime { get; }

    public bool Active { get; } = true;

    public IReadOnlyCollection<CollectedBalanceChallengeParticipation> Participations => _participations;

    public static Result<CollectedBalanceChallenge> Create(
        Guid id,
        CollectedBalanceConstraint constraint,
        DateTime createdDateTime
        )
    {
        var challenge = new CollectedBalanceChallenge(id, constraint, createdDateTime);

        return challenge;
    }

    public Result<CollectedBalanceChallengeParticipation> AddParticipation(CollectedBalanceChallengeParticipation participation)
    {

        if (!participation.ChallengeId.Equals(Id))
        {
            var errors = new[] { "Invalid challenge id associated" };
            return Result<CollectedBalanceChallengeParticipation>.Failure(errors);
        }

        var backwardDays = TimeSpan.FromDays(Constraint.BackwardDayPeriod);

        var periodEnd = participation.OccurredOn;
        var periodStart = periodEnd.Subtract(backwardDays);

        var periodResult = Period.Create(periodStart, periodEnd);

        if (!periodResult.IsSuccess)
        {
            var errors = (string[])periodResult.Errors;
            return Result<CollectedBalanceChallengeParticipation>.Failure(errors);
        }

        _participations.Add(participation);

        var collectedBalance = _participations
            .Where(p =>
                participation.UserId.Equals(p.UserId)
                && periodResult.Value.InRage(p.OccurredOn)
            )
            .Sum(p => p.Amount);

        if (collectedBalance >= Constraint.Amount)
        {
            AddDomainEvent(new AccomplishedCollectedBalanceChallenge());
        }

        return participation;
    }


}
