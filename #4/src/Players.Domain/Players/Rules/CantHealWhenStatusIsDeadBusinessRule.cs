using Players.Domain.Players.Enums;
using Players.Domain.Common;

namespace Players.Domain.Players.Rules;

internal sealed class CantHealWhenStatusIsDeadBusinessRule : IBusinessRule
{
	public string Message { get; private set; } = "Cant heal when dead";
	private readonly Status status;

	public CantHealWhenStatusIsDeadBusinessRule(Status status)
	{
		this.status = status;
	}

	public bool IsBroken()
	{
		if (status == Status.Dead)
			return true;

		return false;
	}
}
