using HexagonalArch.Domain.Errors;
using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Primitives;

public record ChallengeName(string Value)
{
    internal const ushort Length = 50;

    public static Result<ChallengeName> Create(string value)
    {
        if (string.IsNullOrEmpty(value)) return ChallengeNameErrors.NameIsEmpty;

        if (value.Length > Length) return ChallengeNameErrors.CharactersExceeded;

        return new ChallengeName(value);
    }
}
