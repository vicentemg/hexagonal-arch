namespace HexagonalArch.Application.Events;

public interface IIntegrationEvent
{
    Guid EventId { get; }
}
