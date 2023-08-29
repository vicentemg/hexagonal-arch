using HexagonalArch.Domain.SeedWork;
using MediatR;

namespace HexagonalArch.Application.Events.DomainEventHandlers;

public class DomainEventNotification<TEvent> : INotification
where TEvent : IDomainEvent
{
    public DomainEventNotification(TEvent @event)
    {
        Event = @event;
    }
    public TEvent Event { get; }
}