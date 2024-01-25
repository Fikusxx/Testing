using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Players.Api.Tests.Integration.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using System.Data.Common;
using Respawn;
using Npgsql;
using Marten;

namespace Players.Api.Tests.Integration;

public sealed class PlayersControllerApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
	private readonly PostgreSqlContainer postgres = new PostgreSqlBuilder()
		.WithImage("postgres")
		// some fluent With() for debugging purposes
		.Build();

	private DbConnection dbConnection = default!;
	private Respawner respawner = default!;
	public HttpClient HttpClient { get; private set; } = default!;

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureLogging(options =>
		{
			options.ClearProviders();
		});

		builder.ConfigureTestServices(services =>
		{
			// Marten applies migrations with hosted service
			// Either remove all except for Marten's ones OR re-register Marten again after RemoveAll()
			//services.RemoveAll(typeof(IHostedService));

			//services.RemoveAll(typeof(IDbConnectionProvider));
			//services.AddSingleton<IDbConnectionProvider>(_ => new DbConnectionProvider(postgres.GetConnectionString()));
			services.ConfigureMarten(options =>
			{
				options.Connection(postgres.GetConnectionString());
			});

			services.AddAuthentication("TestScheme")
			.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
		});
	}

	public async Task Reset()
	{
		await respawner.ResetAsync(dbConnection);
	}

	public async Task InitializeAsync()
	{
		await postgres.StartAsync();

		dbConnection = new NpgsqlConnection(postgres.GetConnectionString());

		// its important to CreateClient() before opening a connection
		// becuase migrations should be applied, otherwise Respawn throws an error cause of no tables exist
		HttpClient = CreateClient();

		await dbConnection.OpenAsync();

		respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
		{
			DbAdapter = DbAdapter.Postgres,
			SchemasToInclude = ["public"]
		});
	}

	async Task IAsyncLifetime.DisposeAsync()
	{
		await postgres.DisposeAsync().AsTask();
	}
}
