namespace HexagonalArch.Adapter.Persistance.Entities;

public class OutBoxMessage
{
    OutBoxMessage(
        Guid id,
        string type,
        string eventData,
        DateTime occurredOnUtcTime)
    {
        ArgumentException.ThrowIfNullOrEmpty(type);
        ArgumentException.ThrowIfNullOrEmpty(eventData);

        if (id.Equals(Guid.Empty))
        {
            throw new InvalidDataException("Empty guid is not acceptable");
        }

        Id = id;
        Type = type;
        EventData = eventData;
        OccurredOnUtcTime = occurredOnUtcTime;
    }

    public Guid Id { get; }
    public string Type { get; }
    public string EventData { get; }
    public DateTime OccurredOnUtcTime { get; }
}
