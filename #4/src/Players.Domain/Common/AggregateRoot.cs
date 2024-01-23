using System.Runtime.Serialization;


namespace Players.Domain.Common;

public abstract class AggregateRoot : Entity, IAggregateRoot
{
	private readonly List<IDomainEvent> events = new();

	protected void AddDomainEvent(IDomainEvent domainEvent)
	{
		events.Add(domainEvent);
	}

	[IgnoreDataMember]
	public IReadOnlyList<IDomainEvent> DomainEvents => events.AsReadOnly();

	public void ClearDomainEvents()
	{
		events.Clear();
	}
}