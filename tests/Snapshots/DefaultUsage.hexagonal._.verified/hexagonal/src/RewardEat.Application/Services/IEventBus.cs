using RewardEat.Application.Events.Integration;

namespace RewardEat.Application.Services;

public interface IEventBus
{
    Task PublishAsync(IIntegrationEvent @event);

    Task SendAsync(IIntegrationEvent @event);
}
