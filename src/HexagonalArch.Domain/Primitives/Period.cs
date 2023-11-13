using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Primitives;

public record Period(DateTime Start, DateTime End)
{
    public int Days => (End - Start).Days;

    public bool InRage(DateTime dateTime)
    {
        return dateTime >= Start && dateTime <= End;
    }

    public static Result<Period> Create(DateTime dateOne, DateTime dateTwo)
    {
        if (dateOne > dateTwo) return Result<Period>.Failure(new[] { "invalid range of dates" });

        return new Period(dateOne, dateTwo);
    }
}