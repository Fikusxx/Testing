

namespace Players.Api.Tests.Integration.PlayersController;

[Collection(nameof(PlayersControllerFixture))]
public class BasePlayersControllerTests : IAsyncLifetime
{
	protected readonly HttpClient httpClient;
	protected readonly Func<Task> resetDatabase;

	public BasePlayersControllerTests(PlayersControllerApiFactory factory)
	{
		this.httpClient = factory.HttpClient;
		this.resetDatabase = factory.Reset;
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{
		await resetDatabase();
	}
}
