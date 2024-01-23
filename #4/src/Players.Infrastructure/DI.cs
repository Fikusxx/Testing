using Players.Application.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Players.Application.Contracts.Logging;
using Players.Application.Players.Contracts;
using Players.Infrastructure.Services;
using Marten.Events.Daemon.Resiliency;
using Players.Domain.Players;
using Marten;

namespace Players.Infrastructure;

public static class DI
{
	public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
	{
		services.AddScoped<IPlayerRepository, PlayerRepository>();

		services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

		//services.AddSingleton<IDbConnectionProvider, DbConnectionProvider>();

		services.AddMarten(options =>
		{
			options.Connection(EnvironmentExtensions.GetDbConnection());
			options.UseDefaultSerialization(nonPublicMembersStorage: NonPublicMembersStorage.NonPublicConstructor);

			options.RegisterDocumentType<Player>();
		})
		.UseLightweightSessions()
		.ApplyAllDatabaseChangesOnStartup()
		.AddAsyncDaemon(DaemonMode.HotCold);

		return services;
	}
}
