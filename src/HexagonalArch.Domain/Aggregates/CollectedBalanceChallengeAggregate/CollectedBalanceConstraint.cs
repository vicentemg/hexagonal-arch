using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public class CollectedBalanceConstraint : Entity
{
    CollectedBalanceConstraint(
        ushort backwardDayPeriod,
        decimal amount)
    {
        Amount = amount;
        BackwardDayPeriod = backwardDayPeriod;
    }
    public static Result<CollectedBalanceConstraint> Create(ushort backwardDayPeriod, decimal amount)
    {
        if (amount <= 0)
        {
            return Result<CollectedBalanceConstraint>.Failure("Amount should be greater than 0");
        }

        return new CollectedBalanceConstraint(backwardDayPeriod, amount);
    }

    public int Id { get; }
    public ushort BackwardDayPeriod { get; }
    public decimal Amount { get; }
}