using HexagonalArch.Domain.Errors;
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
        if (dateOne > dateTwo) return PeriodErrors.InvalidRange;

        return new Period(dateOne, dateTwo);
    }
}
