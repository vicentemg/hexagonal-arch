using HexagonalArch.Domain.Events;
using HexagonalArch.Domain.Primitives;
using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public class CollectedBalanceChallenge : Entity, IAggregateRoot
{
    private List<CollectedBalanceChallengeParticipation> _participations = new();
    CollectedBalanceChallenge(
        Guid id,
        ChallengeName name,
        CollectedBalanceConstraint constraint,
        DateTime createdDateTime)
    {
        Id = id;
        Name = name;
        Constraint = constraint;
        CreatedDateTime = createdDateTime;

        AddDomainEvent(new CollectedBalanceChallengeCreated(id));

    }

    public Guid Id { get; }
    public ChallengeName Name { get; }
    public CollectedBalanceConstraint Constraint { get; }
    public DateTime CreatedDateTime { get; }
    public bool Active { get; }
    public IReadOnlyCollection<CollectedBalanceChallengeParticipation> Participations => _participations;

    public static Result<CollectedBalanceChallenge> Create(
        Guid id,
        ChallengeName name,
        CollectedBalanceConstraint constraint,
        DateTime createdDateTime
        )
    {
        if (name is null)
        {
            return Result<CollectedBalanceChallenge>.Failure("The challenge name is empty");
        }

        if (constraint is null)
        {
            return Result<CollectedBalanceChallenge>.Failure("The challenge constraint is null");
        }

        return new CollectedBalanceChallenge(id, name, constraint, createdDateTime);
    }

    public Result<CollectedBalanceChallengeParticipation> AddParticipation(CollectedBalanceChallengeParticipation participation)
    {

        if (!participation.ChallengeId.Equals(Id))
        {
            return Result<CollectedBalanceChallengeParticipation>.Failure("Invalid challenge id associated");
        }

        var periodResult = GetPeriodFromParticipation(participation);

        if (!periodResult.IsSuccess)
        {
            return Result<CollectedBalanceChallengeParticipation>.Failure(periodResult.Errors);
        }

        var participationsInRange = _participations
            .Where(p => participation.UserId.Equals(p.UserId)
                && periodResult.Value!.InRage(p.OccurredOn)
            );

        if (participationsInRange.Any(p => p.IsWinner)
            || participation.OccurredOn < CreatedDateTime)
        {
            return participation;
        }

        _participations.Add(participation);

        var collectedBalance = participationsInRange.Sum(p => p.Amount);

        if (collectedBalance >= Constraint.Amount)
        {
            participation.SetAsWinner();
            AddDomainEvent(new AccomplishedCollectedBalanceChallenge());
        }

        return participation;
    }

    private Result<Period> GetPeriodFromParticipation(CollectedBalanceChallengeParticipation participation)
    {
        var backwardDays = TimeSpan.FromDays(Constraint.BackwardDayPeriod);

        var periodEnd = participation.OccurredOn;
        var periodStart = periodEnd.Subtract(backwardDays);

        return Period.Create(periodStart, periodEnd);
    }
}