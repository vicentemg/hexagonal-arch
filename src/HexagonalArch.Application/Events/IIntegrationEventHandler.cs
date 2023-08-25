using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Application.Events;

public interface IIntegrationEventHandler<in TEvent> where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}