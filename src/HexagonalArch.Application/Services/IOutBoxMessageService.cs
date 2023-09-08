using HexagonalArch.Application.Events.Integration;


namespace HexagonalArch.Application.Services;

public interface IOutBoxMessageService
{
    Task AddIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}