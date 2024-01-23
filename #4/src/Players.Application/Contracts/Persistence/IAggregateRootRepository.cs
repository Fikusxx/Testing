using Players.Domain.Common;

namespace Players.Application.Contracts.Persistence;

public interface IAggregateRootRepository<T> where T : IAggregateRoot
{
	public Task StoreAsync(T aggregate, CancellationToken ct = default);
	public Task<T?> LoadAsync(Guid id, CancellationToken ct = default);
}

