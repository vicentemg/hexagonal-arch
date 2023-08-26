namespace HexagonalArch.Application.Events.IntegrationEvents;

public record IntegrationStartedEvent(Guid EventId) : IntegrationEvent(EventId);