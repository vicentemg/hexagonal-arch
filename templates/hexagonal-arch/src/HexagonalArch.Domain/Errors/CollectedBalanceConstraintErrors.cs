using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Errors;

public static class CollectedBalanceConstraintErrors
{
    public static Error InvalidAmount = new(0, "Amount should be greater than 0");
}
