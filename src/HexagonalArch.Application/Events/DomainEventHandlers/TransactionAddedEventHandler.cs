using HexagonalArch.Domain.Events;

namespace HexagonalArch.Application.Events.DomainEventHandlers;

public class TransactionAddedEventHandler : IDomainEventHandler<TransactionAddedEvent>
{
    public Task HandleAsync(TransactionAddedEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
