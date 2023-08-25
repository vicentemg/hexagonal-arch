namespace HexagonalArch.Domain.SeedWork;

public interface IDomainEvent
{
}

public interface IIntegrationEvent : IDomainEvent
{
    Guid EventId { get; }
}
