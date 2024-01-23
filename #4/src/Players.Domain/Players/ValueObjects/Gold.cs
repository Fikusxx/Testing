using Players.Domain.Common.Exceptions;

namespace Players.Domain.Players.ValueObjects;

public sealed record Gold
{
	public int Value { get; private set; }

	private Gold(int value)
	{
		this.Value = value;
	}

	public static Gold CreateNew(int gold)
	{
		if (gold < 0)
			throw new DomainException("Gold cant be below or equal to 0");

		return new Gold(gold);
	}
}
