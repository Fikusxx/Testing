using Players.Domain.Common.Exceptions;
using Players.Domain.Players.ValueObjects;

namespace Players.Domain.Tests.Unit.ValueObjects;

public sealed class HealthTests
{
	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void Constructor_ShouldThrowDomainException_WhenValueIsNegative(int value)
	{
		// Act
		var func = () => Health.CreateNew(value);

		// Assert
		func.Should().Throw<DomainException>()
			.WithMessage("Health cant be below or equal to 0");
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100_000)]
	public void Constructor_ShouldCreateHealth_WhenValueIsPositive(int value)
	{
		// Act
		var sut = Health.CreateNew(value);

		// Assert
		sut.Should().NotBeNull();
		sut.Value.Should().Be(value);
	}
}
