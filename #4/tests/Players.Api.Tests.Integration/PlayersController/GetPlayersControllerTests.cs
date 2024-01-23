using Players.Application.Common.Exceptions;
using Players.Api.Controllers;
using Players.Api.Middlewares;
using System.Net.Http.Json;

namespace Players.Api.Tests.Integration.PlayersController;

public sealed class GetPlayersControllerTests : BasePlayersControllerTests
{
	public GetPlayersControllerTests(PlayersControllerApiFactory factory) : base(factory)	
	{ }

	[Fact]
	public async Task Get_ReturnsNotFound_WhenNoPlayerFound()
	{
		// Arrange
		var id = Guid.NewGuid();

		// Act
		var response = await httpClient.GetAsync($"players/{id}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Get_ReturnsExceptionDetails_WhenNoPlayerFound()
	{
		// Arrange
		var id = Guid.NewGuid();

		// Act
		var response = await httpClient.GetAsync($"players/{id}");
		var result = await response.Content.ReadFromJsonAsync<ExceptionDetails>();

		// Assert
		result.Should().NotBeNull();
		result!.ErrorMessage.Should().Be($"Player with Id: {id} was not found");
		result!.ErrorType.Should().Be(nameof(NotFoundException));
	}

	[Fact]
	public async Task Get_ReturnsOkAndPlayer_WhenPlayerIsFound()
	{
		// Arrange
		var command = new CreatePlayerCommand() { Health = 100, Name = "Fikus" };
		var createdResponse = await httpClient.PostAsJsonAsync("players", command);
		var createdPlayerId = await createdResponse.Content.ReadFromJsonAsync<Guid>();

		// Act
		var response = await httpClient.GetAsync($"players/{createdPlayerId}");

		// Assert
		var retrievedPlayer = await response.Content.ReadFromJsonAsync<PlayerDto>();
		retrievedPlayer!.Id.Should().Be(createdPlayerId);
		retrievedPlayer.Name.Should().Be(command.Name);
		retrievedPlayer.Health.Should().Be(command.Health);
		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task Test1()
	{
		// Arrange

		// Act

		// Assert
	}
}
