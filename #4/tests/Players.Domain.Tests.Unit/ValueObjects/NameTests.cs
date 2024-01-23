using Players.Domain.Players.ValueObjects;
using Players.Domain.Common.Exceptions;

namespace Players.Domain.Tests.Unit.ValueObjects;

public sealed class NameTests
{
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public void Constructor_ShouldThrowDomainException_WhenDataIsInvalid(string value)
	{
		// Act
		var func = () => Name.CreateNew(value);

		// Assert
		func.Should().Throw<DomainException>().WithMessage("Name should be provided"); ;
	}

	[Theory]
	[InlineData("Real Name")]
	[InlineData("Fikus")]
	public void Constructor_ShouldCreateName_WhenDataIsValid(string value)
	{
		// Act
		var sut = Name.CreateNew(value);

		// Assert
		sut.Should().NotBeNull();
		sut.Value.Should().Be(value);
	}
}
