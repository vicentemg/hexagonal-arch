using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Primitives;

public record ChallengeName(string Value)
{
    internal const ushort Length = 50;
    public static Result<ChallengeName> Create(string value)
    {

        if (string.IsNullOrEmpty(value))
        {
            return Result<ChallengeName>.Failure(new[] { "Empty value is not allowed" });
        }

        if (value.Length > Length)
        {
            return Result<ChallengeName>.Failure(new[] { "Characters exceeded" });
        }
 
        return new ChallengeName(value);
    }
}