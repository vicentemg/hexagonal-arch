using RewardEat.Domain.Errors;
using RewardEat.Domain.Events;
using RewardEat.Domain.Primitives;
using RewardEat.Domain.SeedWork;

namespace RewardEat.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public class CollectedBalanceChallenge : Entity, IAggregateRoot
{
    private readonly List<CollectedBalanceChallengeParticipation> _participations = new();

    private CollectedBalanceChallenge(
        Guid id,
        ChallengeName name,
        DateTime createdDateTime)
    {
        Id = id;
        Name = name;
        CreatedDateTime = createdDateTime;
        CollectedBalanceConstraint = null!;
    }

    private CollectedBalanceChallenge(
        Guid id,
        ChallengeName name,
        CollectedBalanceConstraint collectedBalanceConstraint,
        DateTime createdDateTime) : this(id, name, createdDateTime)
    {
        CollectedBalanceConstraint = collectedBalanceConstraint;
        AddDomainEvent(new CollectedBalanceChallengeCreated(id));
    }

    public Guid Id { get; }
    public ChallengeName Name { get; }
    public CollectedBalanceConstraint CollectedBalanceConstraint { get; init; }
    public DateTime CreatedDateTime { get; }
    public bool Active { get; } = false;

    public IReadOnlyCollection<CollectedBalanceChallengeParticipation> Participations => _participations;

    public static Result<CollectedBalanceChallenge> Create(
        Guid id,
        ChallengeName? name,
        CollectedBalanceConstraint? constraint,
        DateTime createdDateTime
    )
    {
        if (constraint is null) return CollectedBalanceChallengeErrors.ChallengeConstraintIsNull;

        if (name is null) return CollectedBalanceChallengeErrors.ChallengeNameIsNull;

        return new CollectedBalanceChallenge(id, name, constraint, createdDateTime);
    }

    public Result<CollectedBalanceChallengeParticipation> AddParticipation(
        CollectedBalanceChallengeParticipation participation)
    {
        if (!participation.ChallengeId.Equals(Id))
            return CollectedBalanceChallengeErrors.InvalidChallengeId;

        var periodResult = GetPeriodFromParticipation(participation);

        if (!periodResult.IsSuccess) return periodResult.Error!;

        var inRangeParticipations = _participations
            .Where(p => participation.UserId.Equals(p.UserId)
                        && periodResult.Value!.InRage(p.OccurredOn)
            );

        if (inRangeParticipations.Any(p => p.IsWinner)
            || participation.OccurredOn < CreatedDateTime)
            return participation;

        _participations.Add(participation);

        var collectedBalance = inRangeParticipations.Sum(p => p.Amount);

        if (collectedBalance >= CollectedBalanceConstraint.Amount)
        {
            participation.SetAsWinner();
            AddDomainEvent(new AccomplishedCollectedBalanceChallenge(Id, participation.UserId));
        }

        return participation;
    }

    private Result<Period> GetPeriodFromParticipation(CollectedBalanceChallengeParticipation participation)
    {
        var backwardDays = TimeSpan.FromDays(CollectedBalanceConstraint.BackwardDayPeriod);

        var periodEnd = participation.OccurredOn;
        var periodStart = periodEnd.Subtract(backwardDays);

        return Period.Create(periodStart, periodEnd);
    }
}
