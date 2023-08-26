namespace HexagonalArch.Application.Providers;
public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime Today { get; }
    DateTime UtcNow { get; }
}
