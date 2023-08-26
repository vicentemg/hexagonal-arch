using HexagonalArch.Application.Events.IntegrationEvents;

namespace HexagonalArch.Application.Services;

public interface IIntegrationEventService
{
    Task SaveAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken);
}