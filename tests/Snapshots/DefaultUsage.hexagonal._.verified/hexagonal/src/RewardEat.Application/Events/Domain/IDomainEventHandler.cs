using RewardEat.Domain.SeedWork;

namespace RewardEat.Application.Events.Domain;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}
