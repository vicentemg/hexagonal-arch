namespace HexagonalArch.Domain.Aggregates.TransactionAggregate;

public class AccumulatedAmountConstraint
{
    public int Id { get; }
    public int BackwardDayPeriod { get; }
    public decimal Amount { get; }
}