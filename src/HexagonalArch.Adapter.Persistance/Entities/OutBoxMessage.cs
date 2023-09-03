namespace HexagonalArch.Adapter.Persistance.Entities;

internal class OutBoxMessage
{
    public Guid Id { get; init; }
    public DateTime OccurredOnUtcTime { get; init; }
    public string Type { get; init; } = default!;
    public string EventData { get; init; } = default!;
}
