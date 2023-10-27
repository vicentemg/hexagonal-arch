namespace HexagonalArch.Adapter.Persistance.Entities;

public class IdempotencyRequest
{
    IdempotencyRequest(Guid id, string name, DateTime occurredOnUtcTime)
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