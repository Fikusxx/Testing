using Players.Domain.Players.ValueObjects;
using Players.Domain.Common;

namespace Players.Domain.Players.Rules;

internal sealed class NotEnoughGoldBusinessRule : IBusinessRule
{
	public string Message { get; private set; } = "Not enough gold";
	private readonly Gold currentGold;
	private readonly int amountToSpend;

	public NotEnoughGoldBusinessRule(Gold currentGold, int amountToSpend)
	{
		this.currentGold = currentGold;
		this.amountToSpend = amountToSpend;
	}

	public bool IsBroken()
	{
		if (currentGold is null)
			return true;

		if (amountToSpend < 0)
			return true;

		if (currentGold.Value - amountToSpend < 0)
			return true;

		return false;
	}
}
