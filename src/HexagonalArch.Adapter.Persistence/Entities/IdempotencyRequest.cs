namespace HexagonalArch.Adapter.Persistence.Entities;

public class IdempotencyRequest
{
    private IdempotencyRequest(Guid id, string name, DateTime occurredOnUtcTime)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Id = id;
        Name = name;
        OccurredOnUtcTime = occurredOnUtcTime;
    }

    public Guid Id { get; }
    public string Name { get; }
    public DateTime OccurredOnUtcTime { get; }
}
