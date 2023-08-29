using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Application.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken);
}
