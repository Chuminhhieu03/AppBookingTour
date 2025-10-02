namespace AppBookingTour.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = null!;

    private readonly List<BaseDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public abstract class BaseDomainEvent
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    public string EventType { get; protected set; } = null!;
}