using RewardEat.Domain.SeedWork;

namespace RewardEat.Domain.Errors;

public static class CollectedBalanceChallengeErrors
{
    public static Error ChallengeConstraintIsNull = new(0, "The Challenge constraint is null");
    public static Error ChallengeNameIsNull = new(0, "The challenge name is empty");
    public static Error InvalidChallengeId = new(0, "The challenge id is invalid");
}
