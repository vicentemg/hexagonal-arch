namespace HexagonalArch.Domain.SeedWork;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public Entity(Guid id)
    {
        Id = id;
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    public Guid Id { get; }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

}