using HexagonalArch.Application.Events.DomainEventHandlers;
using HexagonalArch.Domain.SeedWork;
using MediatR;

namespace HexagonalArch.Application.Events;

public class MediatorDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _publisher;

    public MediatorDomainEventDispatcher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await _publisher.Publish(new DomainEventNotification<IDomainEvent>(domainEvent), cancellationToken);
    }
}