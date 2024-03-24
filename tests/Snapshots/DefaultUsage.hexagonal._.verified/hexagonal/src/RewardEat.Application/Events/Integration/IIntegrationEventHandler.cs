namespace RewardEat.Application.Events.Integration;

public interface IIntegrationEventHandler<TEvent>
    where TEvent : IIntegrationEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}
