using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Application.Bus;

public interface IEventBus
{
    Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}