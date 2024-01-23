

namespace Players.Application.Contracts.Logging;

public interface ILoggerAdapter<TType>
{
	public void LogInformation(string? message, params object?[] args);
	public void LogError(Exception? e, string? message, params object?[] args);
}