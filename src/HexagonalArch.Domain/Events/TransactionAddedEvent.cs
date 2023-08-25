using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Events;
public record TransactionAddedEvent : IIntegrationEvent
{
    public Guid EventId { get; }
}