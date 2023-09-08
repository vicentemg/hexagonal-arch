namespace HexagonalArch.Application.Events.Integration;

public record EarnedPoints(Guid CorrelationId) : IIntegrationEvent;