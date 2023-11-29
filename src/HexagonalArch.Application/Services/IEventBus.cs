using HexagonalArch.Application.Events;

namespace HexagonalArch.Application.Services;

public interface IEventBus
{
    Task PublishAsync(IIntegrationEvent @event);

    Task SendAsync(IIntegrationEvent @event);
}
