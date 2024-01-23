using Players.Domain.Common.Exceptions;
using Players.Domain.Players.Events;
using Players.Domain.Players.Enums;
using Players.Domain.Players;
using Players.Domain.Common;

namespace Players.Domain.Tests.Unit.Entities;

public sealed class PlayerTests
{
	[Fact]
	public void Constructor_ShouldCreatePlayer_WhenDataIsValid()
	{
		// Arrange
		var id = Guid.NewGuid();
		var name = "Fikus";
		var health = 123;

		// Act
		var sut = new Player(id, name, health);

		// Assert
		sut.Should().NotBeNull();
	}

	[Fact]
	public void Constructor_ShouldSetInitialValuesCorrectly_WhenDataIsValid()
	{
		// Arrange
		var id = Guid.NewGuid();
		var name = "Fikus";
		var health = 123;

		// Act
		var sut = new Player(id, name, health);

		// Assert
		sut.Id.Should().Be(id);
		sut.Name.Value.Should().Be(name);
		sut.Health.Value.Should().Be(health);
		sut.Gold.Value.Should().Be(0);
		sut.Status.Should().Be(Status.Alive);
	}

	[Fact]
	public void Constructor_ShouldAddDomainEvent_WhenInvoked()
	{
		// Act
		var sut = new Player(Guid.NewGuid(), "Fikus", 100);
		var events = sut.DomainEvents;
		var @event = events.FirstOrDefault();

		// Assert
		events.Should().NotBeNullOrEmpty();
		events.Should().ContainSingle()
			.Which.Should().BeOfType<PlayerCreatedEvent>()
			.Which.Should().NotBeNull();

		@event.Should().NotBeNull();
		@event.Should().BeOfType<PlayerCreatedEvent>();
		((PlayerCreatedEvent)@event!).Id.Should().Be(sut.Id);
	}

	[Fact]
	public void ClearDomainEvents_ShouldClearEvents_WhenInvoked()
	{
		// Act
		var sut = new Player(Guid.NewGuid(), "Fikus", 100);
		sut.ClearDomainEvents();
		var events = sut.DomainEvents;

		// Assert
		events.Should().BeEmpty();
	}

	[Theory]
	[InlineData("Name #1")]
	[InlineData("Fikus")]
	[InlineData("1231231")]
	public void ChangeName_ShouldChangeName_WhenNameIsValid(string name)
	{
		// Arrange
		var id = Guid.NewGuid();
		var health = 123;
		var initialName = "Some Name";
		var sut = new Player(id, initialName, health);

		// Act
		sut.ChangeName(name);

		// Assert
		sut.Name.Value.Should().Be(name);
	}

	[Fact]
	public void SpendGold_ShouldThrowBusinessRuleValidationException_WhenProvidedNegativeGoldToSpend()
	{
		// Arrange
		var id = Guid.NewGuid();
		var health = 123;
		var name = "Some Name";
		var sut = new Player(id, name, health);

		// Act
		var action = () => sut.SpendGold(-10);

		// Assert
		action.Should().Throw<BusinessRuleValidationException>().WithMessage("Not enough gold");
	}

	[Fact]
	public void SpendGold_ShouldThrowBusinessRuleValidationException_WhenNotEnoughGold()
	{
		// Arrange
		var id = Guid.NewGuid();
		var health = 123;
		var name = "Some Name";
		var sut = new Player(id, name, health);

		// Act
		var action = () => sut.SpendGold(15);

		// Assert
		action.Should().Throw<BusinessRuleValidationException>().WithMessage("Not enough gold");
	}

	[Fact]
	public void Heal_ShouldThrowException_WhenTryingToHealDead()
	{
		// no logic to die sadly
	}
}
