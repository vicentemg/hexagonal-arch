namespace RewardEat.Application.Events;

public interface IEventDispatcher
{
    Task DispatchAsync(object @event, CancellationToken cancellationToken);
}
