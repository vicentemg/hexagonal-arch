using RewardEat.Domain.Errors;
using RewardEat.Domain.SeedWork;

namespace RewardEat.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public class CollectedBalanceConstraint : Entity
{
    private CollectedBalanceConstraint(
        ushort backwardDayPeriod,
        decimal amount)
    {
        Amount = amount;
        BackwardDayPeriod = backwardDayPeriod;
    }

    public int Id { get; }
    public ushort BackwardDayPeriod { get; }
    public decimal Amount { get; }
    public Guid ChallengeId { get; }

    public static Result<CollectedBalanceConstraint> Create(
        ushort backwardDayPeriod,
        decimal amount)
    {
        if (amount <= 0) return CollectedBalanceConstraintErrors.InvalidAmount;

        return new CollectedBalanceConstraint(backwardDayPeriod, amount);
    }
}
