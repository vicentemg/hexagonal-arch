using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public class CollectedBalanceConstraint : Entity
{
    public CollectedBalanceConstraint(
        int id,
        ushort backwardDayPeriod,
        decimal amount)
    {
        Amount = amount;
        Id = id;
        BackwardDayPeriod = backwardDayPeriod;
    }

    public int Id { get; }
    public ushort BackwardDayPeriod { get; }
    public decimal Amount { get; }
}