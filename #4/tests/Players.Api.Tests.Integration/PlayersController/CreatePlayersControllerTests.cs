using Players.Domain.Common.Exceptions;
using Players.Api.Middlewares;
using System.Net.Http.Json;
using FluentAssertions.Execution;

namespace Players.Api.Tests.Integration.PlayersController;

public sealed class CreatePlayersControllerTests : BasePlayersControllerTests
{
	public CreatePlayersControllerTests(PlayersControllerApiFactory factory) : base(factory)
	{ }

	[Fact]
	public async Task Post_ReturnsBadRequest_WhenNameIsInvalid()
	{
		// Arrange
		var command = new CreatePlayerCommand() { Name = "", Health = 100 };

		// Act
		var response = await httpClient.PostAsJsonAsync("players", command);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Post_ReturnsExceptionDetails_WhenNameIsInvalid()
	{
		// Arrange
		var command = new CreatePlayerCommand() { Name = "", Health = 100 };

		// Act
		var response = await httpClient.PostAsJsonAsync("players", command);
		var exDetails = await response.Content.ReadFromJsonAsync<ExceptionDetails>();

		// Assert
		using var _ = new AssertionScope();
		exDetails.Should().NotBeNull();
		exDetails!.ErrorType.Should().Be(nameof(DomainException));
		exDetails!.ErrorMessage.Should().Be("Name should be provided");
		exDetails!.TraceId.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Post_ReturnsBadRequest_WhenHealthIsInvalid()
	{
		// Arrange
		var command = new CreatePlayerCommand() { Name = "Fikus", Health = -100 };

		// Act
		var response = await httpClient.PostAsJsonAsync("players", command);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		var exDetails = await response.Content.ReadFromJsonAsync<ExceptionDetails>();
		exDetails.Should().NotBeNull();
		exDetails!.ErrorType.Should().Be(nameof(DomainException));
		exDetails!.ErrorMessage.Should().Be("Health cant be below or equal to 0");
		exDetails!.TraceId.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Post_ReturnsOkAndCreatesPlayer_WhenDataIsValid()
	{
		// Arrange
		var command = new CreatePlayerCommand() { Name = "Fikus", Health = 100 };

		// Act
		var response = await httpClient.PostAsJsonAsync("players", command);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var createdPlayerId = await response.Content.ReadFromJsonAsync<Guid>();
		createdPlayerId.Should().NotBeEmpty();
	}
}