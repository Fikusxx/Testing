using Players.Application.Players.Contracts;
using Players.Domain.Players;
using Marten;

namespace Players.Infrastructure.Services;

internal sealed class PlayerRepository : IPlayerRepository
{
	private readonly IDocumentSession ctx;

	public PlayerRepository(IDocumentSession ctx)
	{
		this.ctx = ctx;
	}

	public async Task StoreAsync(Player aggregate, CancellationToken ct = default)
	{
		ctx.Store(aggregate);
		await ctx.SaveChangesAsync(ct);
	}

	public async Task<Player?> LoadAsync(Guid id, CancellationToken ct = default)
	{
		var snapshot = await ctx.Query<Player>().FirstOrDefaultAsync(x => x.Id == id, ct);

		return snapshot;
	}
}
