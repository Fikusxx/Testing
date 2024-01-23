using Users.Api.Repositories;
using System.Diagnostics;
using Users.Api.Logging;
using Users.Api.Models;

namespace Users.Api.Services;

public class UserService
{
	private readonly IUserRepository ctx;
	private readonly ILoggerAdapter<UserService> logger;

	public UserService(IUserRepository ctx, ILoggerAdapter<UserService> logger)
	{
		this.ctx = ctx;
		this.logger = logger;
	}

	public async Task<IEnumerable<User>> GetAllAsync()
	{
        logger.LogInformation("Retrieveing all users...");
        var stopWatch = Stopwatch.StartNew();

		try
		{
			return await ctx.GetAllAsync();
		}
		catch (Exception e)
		{
			logger.LogError(e, "Something went wrong...");
			throw;
		}
		finally
		{
			stopWatch.Stop();
			logger.LogInformation("All users retrieved in {0} ms", stopWatch.ElapsedMilliseconds);
		}
	}

	public async Task<User?> GetByIdAsync(Guid id)
	{
		logger.LogInformation($"Retrieveing user with id: {id}");
		var stopWatch = Stopwatch.StartNew();

		try
		{
			return await ctx.GetByIdAsync(id);
		}
		catch (Exception e)
		{
			logger.LogError(e, "Something went wrong...");
			throw;
		}
		finally 
		{
			stopWatch.Stop();
			logger.LogInformation($"All users retrieved in {stopWatch.ElapsedMilliseconds} ms");
		}
	}
}
