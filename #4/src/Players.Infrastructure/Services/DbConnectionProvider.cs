using Marten.Events;
using Players.Application.Contracts.Persistence;

namespace Players.Infrastructure.Services;

public sealed class DbConnectionProvider : IDbConnectionProvider
{
	private readonly string connection;

	public DbConnectionProvider(string? connection = null)
	{
		if (string.IsNullOrWhiteSpace(connection))
			this.connection = "host=localhost;port=5432;database=postgres;password=wc3alive;username=postgres";
		else
			this.connection = connection;
	}

	public string GetDbConnection()
	{
		return connection;
	}
}

public static class EnvironmentExtensions
{
	public static string GetDbConnection()
	{
		var connection = Environment.GetEnvironmentVariable("db_connection");

		if (string.IsNullOrEmpty(connection))
		{
			connection = "host=localhost;port=5432;database=postgres;password=wc3alive;username=postgres";
		}

		return connection;
	}
}
