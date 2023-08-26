using HexagonalArch.Application.Events.IntegrationEvents;

namespace HexagonalArch.Application.Events;

public interface IIntegrationEventHandler<in TEvent> where TEvent : IntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
