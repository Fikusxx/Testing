using NSubstitute.ExceptionExtensions;
using Users.Api.Repositories;
using Users.Api.Services;
using Users.Api.Logging;
using Users.Api.Models;

namespace Users.Api.Tests.Unit;

public class UserServiceTests
{
	private readonly UserService sut;
	private readonly IUserRepository ctx = Substitute.For<IUserRepository>();
	private readonly ILoggerAdapter<UserService> logger = Substitute.For<ILoggerAdapter<UserService>>();

	public UserServiceTests()
	{
		sut = new UserService(ctx, logger);
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
	{
		// Arrange
		ctx.GetAllAsync().Returns(Array.Empty<User>());

		// Act
		var result = await sut.GetAllAsync();

		// Assert
		result.Should().BeEmpty();
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnUsers_WhenUsersExist()
	{
		// Arrange
		var user = new User() { Age = 15, DOB = new(1991, 5, 8), Name = "Fikus" };
		var expected = new List<User>() { user };
		ctx.GetAllAsync().Returns(expected);

		// Act
		var result = await sut.GetAllAsync();

		// Assert
		result.Should().HaveCount(1);
		result.Should().ContainEquivalentOf(user);
		result.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
	{
        // Arrange
        ctx.GetAllAsync().Returns(Enumerable.Empty<User>());

		// Act
		var result = await sut.GetAllAsync();

		// Assert
		logger.Received(1).LogInformation(Arg.Is("Retrieveing all users..."));
		logger.Received(1).LogInformation(Arg.Is<string>(x => x.StartsWith("Retr")));
		logger.Received(1).LogInformation(Arg.Is("All users retrieved in {0} ms"), Arg.Any<long>());
	}

	[Fact]
	public async Task GetAllAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
	{
		// Arrange
		var exception = new Exception();
		ctx.GetAllAsync().Throws(exception);

		// Act
		var result = async () => await sut.GetAllAsync();

		// Assert
		await result.Should().ThrowAsync<Exception>();
		logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong..."));
	}
}
