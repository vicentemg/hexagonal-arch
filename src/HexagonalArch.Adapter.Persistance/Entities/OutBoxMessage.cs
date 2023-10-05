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

        Id = id;
        Type = type;
        EventData = eventData;
        OccurredOnUtcTime = occurredOnUtcTime;
    }

    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public string EventData { get; private set; }
    public DateTime OccurredOnUtcTime { get; private set; }
}
