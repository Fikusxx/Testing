using Players.Domain.Common.Exceptions;

namespace Players.Domain.Players.ValueObjects;

public sealed record Health
{
	public int Value { get; private set; }

	private Health(int value)
	{
		this.Value = value;
	}

	public static Health CreateNew(int health)
	{
		if (health <= 0)
			throw new DomainException("Health cant be below or equal to 0");

		return new Health(health);
	}

	internal Health Heal(int amountToHeal)
	{
		if (amountToHeal < 0)
			throw new DomainException("Cant heal for negative amount");

		return new Health(Value + amountToHeal);
	}
}
