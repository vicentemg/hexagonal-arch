namespace HexagonalArch.Domain.SeedWork;
public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime Today { get; }
    DateTime UtcNow { get; }
}
