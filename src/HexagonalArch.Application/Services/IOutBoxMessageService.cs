using HexagonalArch.Application.Events;

namespace HexagonalArch.Application.Services;

public interface IOutBoxMessageService
{
    Task<Guid> AddIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);

    Task MarkIntegrationEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken);

    Task MarkIntegrationEventAsSuccessAsync(Guid eventId, CancellationToken cancellationToken);

    Task MarkIntegrationEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken);
}
