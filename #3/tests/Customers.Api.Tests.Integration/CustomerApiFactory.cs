using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Customers.Api.Tests.Integration;

public class CustomerApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
	private readonly PostgreSqlContainer postgres = new PostgreSqlBuilder()
		.WithImage("postgres")
		//.WithEnvironment("POSTGRES_USER", "postgres")
		//.WithEnvironment("POSTGRES_PASSWORD", "whatever")
		//.WithEnvironment("POSTGRES_DB", "testdb")
		//.WithPortBinding(5555, 5432)
		//.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
		.Build();

	private readonly MockedServer mockedServer = new();

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureLogging(options =>
		{
			options.ClearProviders(); // disable logging
		});

		builder.ConfigureTestServices(services => 
		{
			// remove background service to not mess up the tests
			services.RemoveAll(typeof(IHostedService));

			// change connection to that container's data
			// services.RemoveAll(typeof(IDbConnectionFactory));
			// services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(postgres.GetConnectionString()));

			// swap original httpClient registration with mocked one
			services.AddHttpClient("Mocked", client =>
			{
				client.BaseAddress = new Uri(mockedServer.Url);
			});
		});
	}

	public async Task InitializeAsync()
	{
		await postgres.StartAsync();
		mockedServer.Start();
		mockedServer.SetupUser("Fikus");
		mockedServer.SetupThrottledUser("Throttled");
	}

	public new async Task DisposeAsync()
	{
		mockedServer.Dispose();
		await postgres.DisposeAsync().AsTask();
	}
}
