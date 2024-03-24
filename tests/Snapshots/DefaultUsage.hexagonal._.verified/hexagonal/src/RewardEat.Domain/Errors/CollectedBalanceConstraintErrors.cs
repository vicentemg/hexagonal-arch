using RewardEat.Domain.SeedWork;

namespace RewardEat.Domain.Errors;

public static class CollectedBalanceConstraintErrors
{
    public static Error InvalidAmount = new(0, "Amount should be greater than 0");
}
