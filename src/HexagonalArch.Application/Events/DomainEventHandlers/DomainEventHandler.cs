using HexagonalArch.Domain.SeedWork;
using MediatR;

namespace HexagonalArch.Application.Events.DomainEventHandlers;

public abstract class DomainEventHandler<TEvent> : INotificationHandler<DomainEventNotification<TEvent>>
where TEvent : IDomainEvent
{
    public abstract Task Handle(DomainEventNotification<TEvent> notification, CancellationToken cancellationToken);
}
