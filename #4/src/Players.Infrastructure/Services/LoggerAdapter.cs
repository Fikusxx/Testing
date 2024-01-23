using Players.Application.Contracts.Logging;
using Microsoft.Extensions.Logging;

namespace Players.Infrastructure.Services;

internal sealed class LoggerAdapter<TType> : ILoggerAdapter<TType>
{
	private readonly ILogger<TType> logger;

	public LoggerAdapter(ILogger<TType> logger)
	{
		this.logger = logger;
	}

	public void LogError(Exception? e, string? message, params object?[] args)
	{
		logger.LogError(e, message, args);
	}

	public void LogInformation(string? message, params object?[] args)
	{
		logger.LogInformation(message, args);
	}
}
