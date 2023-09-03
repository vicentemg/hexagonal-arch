using HexagonalArch.Application.Events.Integration;


namespace HexagonalArch.Application.Services;

public interface IOutBoxMessageRepository
{
    Task AddIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}