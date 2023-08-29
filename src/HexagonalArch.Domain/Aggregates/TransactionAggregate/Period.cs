using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Aggregates.TransactionAggregate;

public record Period(DateTime Start, DateTime End)
{
    public int Days => (Start - End).Days;

    public static Result<Period> Create(DateTime dateOne, DateTime dateTwo)
    {
        if (dateOne > dateTwo)
        {
            return Result<Period>.Failure(new[] { "invalid range of dates" });
        }

        return Result<Period>.Success(new(dateOne, dateTwo));
    }
}