namespace HexagonalArch.Application.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(object @event, CancellationToken cancellationToken);
}

public interface IIntegrationEventDispatcher : IDomainEventDispatcher
{
}