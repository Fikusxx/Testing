using Players.Application.Contracts.Logging;
using Players.Application.Players.Contracts;
using Players.Domain.Players;

namespace Players.Application.Tests.Unit.Players.Commands;

public sealed class CreatePlayerCommandHandlerTests
{
    private readonly IPlayerRepository ctx = Substitute.For<IPlayerRepository>();
    private readonly ILoggerAdapter<CreatePlayerCommandHandler> logger = Substitute.For<ILoggerAdapter<CreatePlayerCommandHandler>>();
    private readonly CreatePlayerCommand command;
    private readonly CreatePlayerCommandHandler sut;

    public CreatePlayerCommandHandlerTests()
    {
        command = new CreatePlayerCommand { Name = "Fikus", Health = 100 };
        sut = new CreatePlayerCommandHandler(ctx, logger);
	}

    [Fact]
    public async Task Handle_ShouldReturnId_WhenDataIsValid()
    {
        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldLogMessage_WhenInvoked()
    {
        // Act
        await sut.Handle(command, default);

        // Assert
        logger.Received(1).LogInformation("Creating player");
    }

    [Fact]
    public async Task Handle_ShouldInvokeRepository_WhenInvoked()
    {
		// Act
		var result = await sut.Handle(command, default);

        // Assert
        await ctx.Received(1).StoreAsync(Arg.Is<Player>(x => x.Id == result), Arg.Any<CancellationToken>());
	}
}
