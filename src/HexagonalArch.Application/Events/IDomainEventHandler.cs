using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Application.Events;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}