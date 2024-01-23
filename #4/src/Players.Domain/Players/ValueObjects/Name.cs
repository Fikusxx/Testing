using Players.Domain.Common.Exceptions;

namespace Players.Domain.Players.ValueObjects;

public sealed record Name
{
	public string Value { get; private set; }

	private Name(string value)
	{
		this.Value = value;
	}

	public static Name CreateNew(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new DomainException("Name should be provided");

		return new Name(name);
	}
}
