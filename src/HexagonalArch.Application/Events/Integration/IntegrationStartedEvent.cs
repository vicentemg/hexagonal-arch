namespace HexagonalArch.Application.Events.Integration;

public record PointsEarned(Guid CorrelationId) : IIntegrationEvent;