using Players.Application.Common.Exceptions;
using Players.Application.Contracts.Logging;
using Players.Application.Players.Contracts;
using Players.Application.Players.Queries;
using Players.Domain.Players;

namespace Players.Application.Tests.Unit.Players.Queries;

public sealed class GetByIdQueryHandlerTests
{
    private readonly IPlayerRepository ctx = Substitute.For<IPlayerRepository>();
    private readonly ILoggerAdapter<GetByIdQueryHandler> logger = Substitute.For<ILoggerAdapter<GetByIdQueryHandler>>();
    private readonly GetByIdQuery query;
    private readonly GetByIdQueryHandler sut;

    public GetByIdQueryHandlerTests()
    {
        query = new GetByIdQuery() { Id = Guid.NewGuid() };
		sut = new GetByIdQueryHandler(ctx, logger);
    }

    [Fact]
    public async Task Handle_ShouldThrowAndLogError_WhenPlayerIsNotFound()
    {
        // Arrange
        ctx.LoadAsync(query.Id).Returns((Player)null!);

        // Act
        var func = async () => await sut.Handle(query, default);

        // Assert
        await func.Should().ThrowAsync<NotFoundException>().WithMessage($"Player with Id: {query.Id} was not found");
        logger.Received(1).LogInformation("Player with Id: {Id} was not found", Arg.Is(query.Id));
    }

    [Fact]
    public async Task Handle_ShouldProcessAndLogMessage_WhenPlayerIsFound()
    {
        // Arrange
        ctx.LoadAsync(query.Id).Returns(new Player(query.Id, "Fikus", 100));

        // Act
        var func = async () => await sut.Handle(query, default);

        // Assert
        await func.Should().NotThrowAsync();
        logger.Received(1).LogInformation("Getting player");
    }

    [Fact]
    public async Task Handle_ShouldReturnPlayer_WhenPlayerIsFound()
    {
        // Arrange
        var player = new Player(query.Id, "Fikus", 100);
        ctx.LoadAsync(query.Id).Returns(player);

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.Should().Be(player);
    }

    [Fact]
    public async Task Handle_ShouldInvokeRepository_WhenInvoked()
    {
		// Arrange
		var player = new Player(query.Id, "Fikus", 100);
		ctx.LoadAsync(query.Id).Returns(player);

		// Act
		await sut.Handle(query, default);

        // Assert
        await ctx.Received(1).LoadAsync(Arg.Is(query.Id), Arg.Any<CancellationToken>());
	}

    [Fact]
    public async Task Test1()
    {
        // Arrange

        // Act

        // Assert
    }
}
