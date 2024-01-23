using Players.Domain.Players.ValueObjects;
using System.Text.Json.Serialization;
using Players.Domain.Players.Events;
using Players.Domain.Players.Enums;
using Players.Domain.Players.Rules;
using Players.Domain.Common;

namespace Players.Domain.Players;


public sealed class Player : AggregateRoot
{
	public Name Name { get; private set; }
	public Health Health { get; private set; }
	public Gold Gold { get; private set; }
	public Status Status { get; private set; }

	public Player(Guid id, string name, int health)
	{
		Id = id;
		Name = Name.CreateNew(name);
		Health = Health.CreateNew(health);
		Gold = Gold.CreateNew(0);

		AddDomainEvent(new PlayerCreatedEvent() { Id = this.Id });
	}

	[JsonConstructor]
	internal Player(Guid id, Name name, Health health, Gold gold, Status status)
	{
		Id = id;
		Name = name;
		Health = health;
		Gold = gold;
		Status = status;
	}

	public void ChangeName(string name)
	{
		Name = Name.CreateNew(name);
	}

	public void Heal(int amount)
	{
		CheckRule(new CantHealWhenStatusIsDeadBusinessRule(Status));

		Health = Health.Heal(amount);
	}

	public void SpendGold(int amount)
	{
		CheckRule(new NotEnoughGoldBusinessRule(Gold, amount));

		Gold = Gold.CreateNew(Gold.Value - amount);
	}
}
