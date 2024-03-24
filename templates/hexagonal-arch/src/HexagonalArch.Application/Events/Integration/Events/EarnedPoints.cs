namespace HexagonalArch.Application.Events.Integration.Events;

public record EarnedPoints(Guid EventId, Guid UserId, int Points) : IIntegrationEvent;
