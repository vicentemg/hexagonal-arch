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

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTime OccurredOnUtcTime { get; private set; }
}