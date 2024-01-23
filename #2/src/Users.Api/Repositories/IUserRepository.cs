using Users.Api.Models;

namespace Users.Api.Repositories;

public interface IUserRepository
{
	public Task<IEnumerable<User>> GetAllAsync();
	public Task<User?> GetByIdAsync(Guid id);
}
