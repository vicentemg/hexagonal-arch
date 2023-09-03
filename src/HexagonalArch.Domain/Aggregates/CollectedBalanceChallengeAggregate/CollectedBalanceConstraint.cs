namespace HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

public class CollectedBalanceConstraint
{
    public CollectedBalanceConstraint(int backwardDayPeriod, decimal amount)
    {
        Amount = amount;
        BackwardDayPeriod = backwardDayPeriod;
    }

    public int Id { get; }
    public int BackwardDayPeriod { get; }
    public decimal Amount { get; }
}