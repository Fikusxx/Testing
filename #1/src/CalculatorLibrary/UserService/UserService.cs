

namespace Library;

public class UserService
{
	private readonly IUserRepository ctx;

	public UserService(IUserRepository ctx)
	{
		this.ctx = ctx;
	}

	public async Task<IEnumerable<User>> GetAllAsync()
	{
		return await ctx.GetAllAsync();
	}
}

public interface IUserRepository
{
	public Task<IEnumerable<User>> GetAllAsync();
}
