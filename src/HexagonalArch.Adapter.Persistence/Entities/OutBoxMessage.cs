using HexagonalArch.Application.Services;

namespace HexagonalArch.Adapter.Persistence.Entities;

public class OutBoxMessage
{
    public OutBoxMessage(
        Guid id,
        string type,
        string eventData,
        DateTime occurredOnUtcTime)
    {
        ArgumentException.ThrowIfNullOrEmpty(type);
        ArgumentException.ThrowIfNullOrEmpty(eventData);

        if (id.Equals(Guid.Empty)) throw new InvalidDataException("Empty guid is not acceptable");

        Id = id;
        Type = type;
        Attempts = 0;
        EventData = eventData;
        Status = OutBoxMessageStatus.Pending;
        OccurredOnUtcTime = occurredOnUtcTime;
    }

    public Guid Id { get; }
    public string Type { get; }
    public string EventData { get; }
    public DateTime OccurredOnUtcTime { get; }
    public OutBoxMessageStatus Status { get; set; }
    public int Attempts { get; set; }
}