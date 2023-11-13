using HexagonalArch.Domain.Events;
using HexagonalArch.Domain.Primitives;
using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

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
        if (constraint is null) return Result<CollectedBalanceChallenge>.Failure("The challenge constraint is null");

        if (name is null) return Result<CollectedBalanceChallenge>.Failure("The challenge name is empty");


        return new CollectedBalanceChallenge(id, name, constraint, createdDateTime);
    }

    public Result<CollectedBalanceChallengeParticipation> AddParticipation(
        CollectedBalanceChallengeParticipation participation)
    {
        if (!participation.ChallengeId.Equals(Id))
            return Result<CollectedBalanceChallengeParticipation>.Failure("Invalid challenge id associated");

        var periodResult = GetPeriodFromParticipation(participation);

        if (!periodResult.IsSuccess) return Result<CollectedBalanceChallengeParticipation>.Failure(periodResult.Errors);

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