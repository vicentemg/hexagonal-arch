using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Errors;

public static class ChallengeNameErrors
{
    public static Error NameIsEmpty = new(0, "The challenge name is empty");
    public static Error CharactersExceeded = new(0, "The challenge name has more than 50 characters");
}
