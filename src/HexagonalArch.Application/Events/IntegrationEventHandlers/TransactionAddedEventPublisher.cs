using HexagonalArch.Application.Bus;
using HexagonalArch.Domain.Events;

namespace HexagonalArch.Application.Events.IntegrationEventHandlers;

public class TransactionAddedEventPublisher : IIntegrationEventHandler<TransactionAddedEvent>
{
    private readonly IEventBus _eventBus;

    public TransactionAddedEventPublisher(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task HandleAsync(TransactionAddedEvent @event, CancellationToken cancellationToken)
    {
        return _eventBus.PublishAsync(@event, cancellationToken);
    }
}
