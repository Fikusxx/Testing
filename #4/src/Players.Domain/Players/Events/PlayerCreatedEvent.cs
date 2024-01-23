using Players.Domain.Common;

namespace Players.Domain.Players.Events;

public sealed record PlayerCreatedEvent : IDomainEvent
{
	public required Guid Id { get; init; }
}
