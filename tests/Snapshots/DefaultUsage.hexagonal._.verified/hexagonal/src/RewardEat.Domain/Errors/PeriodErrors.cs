using RewardEat.Domain.SeedWork;

namespace RewardEat.Domain.Errors;

public static class PeriodErrors
{
    public static Error InvalidRange = new(0, "invalid range of dates");
}
