

namespace Players.Application.Contracts.Persistence;

public interface IDbConnectionProvider
{
	public string GetDbConnection();
}
