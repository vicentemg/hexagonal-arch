namespace HexagonalArch.Application.Events;

public interface IIntegrationEventHandler<TEvent>
    where TEvent : IIntegrationEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}
