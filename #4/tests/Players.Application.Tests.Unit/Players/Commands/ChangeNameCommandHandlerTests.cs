using Players.Application.Common.Exceptions;
using Players.Application.Contracts.Logging;
using Players.Application.Players.Contracts;
using Players.Domain.Players;

namespace Players.Application.Tests.Unit.Players.Commands;

public sealed class ChangeNameCommandHandlerTests
{
	private readonly IPlayerRepository ctx = Substitute.For<IPlayerRepository>();
	private readonly ILoggerAdapter<ChangeNameCommandHandler> logger = Substitute.For<ILoggerAdapter<ChangeNameCommandHandler>>();
	private readonly ChangeNameCommand command;
	private readonly ChangeNameCommandHandler sut;

	public ChangeNameCommandHandlerTests()
	{
		command = new ChangeNameCommand { Id = Guid.NewGuid(), Name = "Fikus" };
		sut = new ChangeNameCommandHandler(ctx, logger);
	}

	[Fact]
	public async Task Handle_ShouldThrowAndLogError_WhenPlayerIsNotFound()
	{
		// Arrange
		ctx.LoadAsync(command.Id).Returns((Player)null!);

		// Act
		var func = async () => await sut.Handle(command, default);

		// Assert
		await func.Should().ThrowAsync<NotFoundException>().WithMessage("Player with was not found");
		logger.Received(1).LogInformation("Player with Id: {Id} was not found", Arg.Is(command.Id));
	}

	[Fact]
	public async Task Handle_ShouldProcessAndLogMessage_WhenPlayerIsFound()
	{
		// Arrange
		ctx.LoadAsync(command.Id).Returns(new Player(command.Id, "Vasya", 100));

		// Act
		var action = async () => await sut.Handle(command, default);

		// Assert
		await action.Should().NotThrowAsync();
		logger.Received(1).LogInformation("Changing name to {Name}", Arg.Is(command.Name));
	}

	[Fact]
	public async Task Handle_ShouldInvokeRepository_WhenPlayerIsFound()
	{
		// Arrange
		ctx.LoadAsync(command.Id).Returns(new Player(command.Id, "Vasya", 100));

		// Act
		await sut.Handle(command, default);

		// Assert
		await ctx.Received(1).LoadAsync(Arg.Is(command.Id), Arg.Any<CancellationToken>());
		await ctx.Received(1).StoreAsync(Arg.Is<Player>(x => x.Id == command.Id), Arg.Any<CancellationToken>());
	}
}
